using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UMCFormTest.Database;
using UMCFormTest.Helper;
using UMCFormTest.Models;

namespace UMCFormTest.Controllers
{
    public class HomeController : Controller
    {
        public STAFF user = SessionHelper.Get<STAFF>(Constant.SESSION_LOGIN);
        public ActionResult Index()
        {
            try
            {
                if (user == null) return View("Login");
                using (var db = new UMC_TESTEntities())
                {
                    var exam = db.EXAMs.Include("QUESTIONs").OrderByDescending(m => m.DateCreate).Where(m => m.IsCurrent == true).FirstOrDefault();
                    var userTest = db.USER_TEST.Where(m => m.ID_Exam == exam.ID && m.StaffCode == user.StaffCode).FirstOrDefault();
                    if (userTest == null)
                    {
                        return View(exam);
                    }
                    else
                    {
                        return RedirectToAction("Detail", new { Id = userTest.ID });
                    }

                }
            }
            catch (Exception)
            {
                return View();
            }


        }

        public ActionResult Detail(int Id)
        {
            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                var userTest = db.USER_TEST.Include("EXAM").Include("USER_TEST_DETAIL").Include("STAFF").Where(m => m.ID == Id).FirstOrDefault();
                if (userTest == null)
                {
                    return RedirectToAction("ListTest");
                }
                else if (userTest.StaffCode != user.StaffCode)
                {
                    return RedirectToAction("Index");
                }
                userTest.USER_TEST_DETAIL = db.USER_TEST_DETAIL.Include("QUESTION").Where(m => m.ID_USER_TEST == Id).ToList();
                return View(userTest);
            }
        }
        public ActionResult Login()
        {

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            ViewBag.OldPassword = user.Password;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(STAFF member, string old_Password, string reenter_Password)
        {
            try
            {
                using (var db = new UMC_TESTEntities())
                {
                    var u = db.STAFFs.Where(m => m.StaffCode == user.StaffCode).FirstOrDefault();
                    if (u != null)
                    {
                        if (old_Password != u.Password)
                        {
                            ModelState.AddModelError("name", "Mật khẩu cũ nhập sai!");
                            return View();
                        }
                        else if (member.Password != reenter_Password)
                        {
                            ModelState.AddModelError("name", "Nhập lại mật khẩu không trùng khớp!");
                            return View();
                        }

                        u.Password = member.Password;
                        db.SaveChanges();
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Không tồn tại user này!");
                        return View("Login");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", "Kiểm tra lại Code hoặc Password!");
                return View("Login");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(STAFF member)
        {
            try
            {
                using (var db = new UMC_TESTEntities())
                {
                    var user = db.STAFFs.Where(m => m.StaffCode == member.StaffCode && m.Password == member.Password).FirstOrDefault();
                    if (user != null)
                    {
                        SessionHelper.Set(Constant.SESSION_LOGIN, user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Kiểm tra lại Code hoặc Password!");
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", "Kiểm tra lại Code hoặc Password!");
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(STAFF member)
        {
            try
            {
                using (var db = new UMC_TESTEntities())
                {
                    var user = db.STAFFs.Where(m => m.StaffCode == member.StaffCode).FirstOrDefault();
                    if (user == null)
                    {
                        user = new STAFF()
                        {
                            StaffCode = member.StaffCode,
                            Password = member.Password,
                            Dept = member.Dept,
                            FullName = member.FullName,
                            IsReviewer = member.IsReviewer
                        };
                        db.STAFFs.Add(user);
                        db.SaveChanges();
                        ViewBag.Message = "Bạn đã tạo xong user " + user.StaffCode + "!";
                        return View("Success");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "User này đã đăng kí rồi!");
                        return View("Index");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Có lỗi xảy ra!");
                return View("Index");
            }

        }
        public ActionResult Logout()
        {
            if (SessionHelper.Get<STAFF>(Constant.SESSION_LOGIN) != null)
            {
                SessionHelper.Remove(Constant.SESSION_LOGIN);
            }
            return View("Login");
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Submit(string StaffCode, string DateTest, int ID_Exam, string answers)
        {

            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var userTest = db.USER_TEST.Where(m => m.ID_Exam == ID_Exam && m.StaffCode == StaffCode).FirstOrDefault();
                        if (userTest != null)
                        {
                            ViewBag.Message = "Bạn đã làm bài thi này rồi!";
                            return View("Error");
                        }
                        var list = JsonConvert.DeserializeObject<List<Answer>>(answers);
                        userTest = new USER_TEST
                        {
                            StaffCode = StaffCode,
                            DateTest = DateTime.Parse(DateTest),
                            ID_Exam = ID_Exam
                        };
                        userTest = db.USER_TEST.Add(userTest);
                        db.SaveChanges();
                        if (userTest.ID == 0)
                        {
                            return View("Error");
                        }
                        List<USER_TEST_DETAIL> answerList = new List<USER_TEST_DETAIL>();
                        foreach (var answer in list)
                        {
                            var userTestDetail = new USER_TEST_DETAIL()
                            {
                                ID_QUESTION = answer.index,
                                ID_USER_TEST = userTest.ID,
                                ANSWER = answer.value
                            };
                            answerList.Add(userTestDetail);
                        }
                        db.USER_TEST_DETAIL.AddRange(answerList);
                        db.SaveChanges();
                        transaction.Commit();
                        ViewBag.Message = "Bạn đã hoàn thành xong bài thi!";
                        return View("Success");


                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ViewBag.Message = "Có lỗi xảy ra!";
                        return View("Error");
                    }

                }

            }
        }
        public JsonResult IsExamDoing(string Id)
        {
            try
            {
                var user = SessionHelper.Get<STAFF>(Constant.SESSION_LOGIN);
                if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
                using (var db = new UMC_TESTEntities())
                {
                    if (string.IsNullOrEmpty(Id))
                    {
                        return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int id = int.Parse(Id);
                        var userTest = db.USER_TEST.Where(m => m.ID_Exam == id).FirstOrDefault();
                        if (userTest != null)
                        {
                            return Json(new { result = "IS_DOING" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new { result = "NOT_DOING" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception e)
            {
                return Json(new { body = "" }, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public ActionResult DeleteExam(int ID_Exam)
        {
            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                var exam = db.USER_TEST.Where(m => m.ID_Exam == ID_Exam).FirstOrDefault();
                if (exam == null)
                {
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var listQuest = db.QUESTIONs.Where(m => m.ID_EXAM == ID_Exam).ToList();
                            db.QUESTIONs.RemoveRange(listQuest);
                            var examDb = db.EXAMs.Where(m => m.ID == ID_Exam).FirstOrDefault();
                            db.EXAMs.Remove(examDb);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            ViewBag.Message = "Có lỗi xảy ra!";
                            return View("Error");
                        }
                    }
                }
                else
                {

                }
                return RedirectToAction("ListExam");
            }
        }
        [HttpPost]
        public ActionResult DeleteExamWhenDoing(int ID_Exam)
        {
            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var userTest = db.USER_TEST.Where(m => m.ID_Exam == ID_Exam).ToList();
                        foreach (var user in userTest)
                        {
                            var userTestDetail = db.USER_TEST_DETAIL.Where(m => m.ID_USER_TEST == user.ID).ToList();
                            db.USER_TEST_DETAIL.RemoveRange(userTestDetail);

                        }
                        db.SaveChanges();
                        db.USER_TEST.RemoveRange(userTest);
                        db.SaveChanges();
                        var listQuest = db.QUESTIONs.Where(m => m.ID_EXAM == ID_Exam).ToList();
                        db.QUESTIONs.RemoveRange(listQuest);
                        var examDb = db.EXAMs.Where(m => m.ID == ID_Exam).FirstOrDefault();
                        db.EXAMs.Remove(examDb);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        ViewBag.Message = "Có lỗi xảy ra!";
                        return View("Error");
                    }

                }
                return RedirectToAction("ListExam");
            }
        }

        public ActionResult ListTest()
        {
            if (user == null) return View("Login");
            if (!user.IsReviewer) return RedirectToAction("Index");
            using (var db = new UMC_TESTEntities())
            {
                var list = db.USER_TEST.Include("EXAM").Include("STAFF").Include("USER_TEST_DETAIL").OrderByDescending(m => m.DateTest).ToList();
                return View(list);
            }

        }

        [HttpPost]

        public ActionResult Mark(int point, string review, int ID_USER_TEST)
        {
            try
            {
                if (user == null) return View("Login");
                using (var db = new UMC_TESTEntities())
                {
                    var userTest = db.USER_TEST.Where(m => m.ID == ID_USER_TEST).FirstOrDefault();
                    if (userTest == null)
                    {
                        return View("Error");
                    }
                    userTest.Mark = point;
                    userTest.Review = review;
                    db.SaveChanges();

                }
                return RedirectToAction("ListTest");
            }
            catch (Exception)
            {
                return View("Error");
            }

        }

        public ActionResult ListExam()
        {
            if (user == null) return View("Login");
            if (!user.IsReviewer) return RedirectToAction("Index");
            using (var db = new UMC_TESTEntities())
            {
                var list = db.EXAMs.ToList();
                return View(list);
            }
        }

        [HttpPost]

        public ActionResult AddExam(string name_exam_vi, string name_exam_ja, string list_question, string isCurrent, string target, string id_exam)
        {
            try
            {
                if (user == null) return View("Login");
                using (var db = new UMC_TESTEntities())
                {

                    var list = JsonConvert.DeserializeObject<List<Question>>(list_question);
                    var examDb = new EXAM();
                    if (!string.IsNullOrEmpty(id_exam))
                    {
                        var id = int.Parse(id_exam);

                        var userTest = db.USER_TEST.Where(m => m.ID_Exam == id).FirstOrDefault();
                        if (userTest != null)
                        {
                            if (isCurrent == "true")
                            {
                                examDb = db.EXAMs.Where(m => m.ID == id).FirstOrDefault();
                                examDb.IsCurrent = isCurrent == "true" ? true : false;
                                db.SaveChanges();
                                var listExam = db.EXAMs.Where(m => m.IsCurrent == true && m.ID != examDb.ID).ToList();
                                foreach (var e in listExam)
                                {
                                    e.IsCurrent = false;
                                    db.SaveChanges();
                                }
                            }
                            return RedirectToAction("ListExam");
                        }

                        examDb = db.EXAMs.Where(m => m.ID == id).FirstOrDefault();
                        examDb.Name = JsonConvert.SerializeObject(new Question()
                        {
                            vi = name_exam_vi,
                            ja = name_exam_ja
                        });
                        examDb.DateCreate = DateTime.Now;
                        examDb.IsCurrent = isCurrent == "true" ? true : false;
                        examDb.Target = int.Parse(target);
                        db.SaveChanges();
                        var ques = db.QUESTIONs.Where(m => m.ID_EXAM == examDb.ID).ToList();
                        foreach (var item in ques)
                        {
                            db.QUESTIONs.Remove(item);
                        }
                    }
                    else
                    {
                        var exam = new EXAM()
                        {
                            Name = JsonConvert.SerializeObject(new Question()
                            {
                                vi = name_exam_vi,
                                ja = name_exam_ja
                            }),
                            DateCreate = DateTime.Now,
                            IsCurrent = isCurrent == "true" ? true : false,
                            Target = int.Parse(target)
                        };
                        examDb = db.EXAMs.Add(exam);
                        db.SaveChanges();

                    }

                    if (examDb.IsCurrent == true)
                    {
                        var listExam = db.EXAMs.Where(m => m.IsCurrent == true && m.ID != examDb.ID).ToList();
                        foreach (var e in listExam)
                        {
                            e.IsCurrent = false;
                            db.SaveChanges();
                        }
                    }
                    foreach (var question in list)
                    {
                        var ques = new QUESTION()
                        {
                            ID_EXAM = examDb.ID,
                            Question1 = JsonConvert.SerializeObject(question),
                            DateCreate = DateTime.Now
                        };
                        db.QUESTIONs.Add(ques);
                        db.SaveChanges();
                    }
                    return RedirectToAction("ListExam");




                }

            }
            catch (Exception e)
            {
                ViewBag.Messagee = e.Message.ToString();
                return View("Error");
            }

        }


        public JsonResult GetExamDetail(string Id)
        {
            try
            {
                var user = SessionHelper.Get<STAFF>(Constant.SESSION_LOGIN);
                if (user == null) return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
                using (var db = new UMC_TESTEntities())
                {
                    if (string.IsNullOrEmpty(Id))
                    {
                        return Json(new { body = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        int id = int.Parse(Id);
                        db.Configuration.ProxyCreationEnabled = false;
                        var exam = db.EXAMs.Where(m => m.ID == id).FirstOrDefault();
                        var question = db.QUESTIONs.Where(m => m.ID_EXAM == exam.ID).Select(p => new
                        {
                            ques = p.Question1
                        }).ToList();
                        var userTest = db.USER_TEST.Where(m => m.ID_Exam == id).Select(p => new
                        {
                            id = p.ID
                        }).ToList();
                        if (userTest != null && userTest.Count > 0)
                        {
                            return Json(new { exam = exam, question = question, isEdit = false }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new { exam = exam, question = question, isEdit = true }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception e)
            {
                return Json(new { body = "" }, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult ShowPDF(int Id)
        {
            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                var userTest = db.USER_TEST.Include("EXAM").Include("USER_TEST_DETAIL").Include("STAFF").Where(m => m.ID == Id).FirstOrDefault();
                if (userTest == null)
                {
                    return RedirectToAction("Index");
                }
                userTest.USER_TEST_DETAIL = db.USER_TEST_DETAIL.Include("QUESTION").Where(m => m.ID_USER_TEST == Id).ToList();
                return View(userTest);
            }
        }

    }
}