using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            using (var db = new UMC_TESTEntities())
            {
                var userTest = db.USER_TEST.Include("EXAM").Include("USER_TEST_DETAIL").Include("STAFF").Where(m => m.ID == Id).FirstOrDefault();
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
            try
            {
                if (user == null) return View("Login");
                using (var db = new UMC_TESTEntities())
                {
                    var list = JsonConvert.DeserializeObject<List<Answer>>(answers);
                    var userTest = new USER_TEST
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
                    foreach (var answer in list)
                    {
                        var userTestDetail = new USER_TEST_DETAIL()
                        {
                            ID_QUESTION = answer.index,
                            ID_USER_TEST = userTest.ID,
                            ANSWER = answer.value
                        };
                        db.USER_TEST_DETAIL.Add(userTestDetail);
                        db.SaveChanges();
                    }
                    ViewBag.Message = "Bạn đã hoàn thành xong bài thi!";
                    return View("Success");
                }
            }
            catch (Exception e)
            {
                return View("Error");
            }

        }

        public ActionResult ListTest()
        {
            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                var list = db.USER_TEST.Include("EXAM").Include("STAFF").Include("USER_TEST_DETAIL").ToList();
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
            using (var db = new UMC_TESTEntities())
            {
                var list = db.EXAMs.ToList();
                return View(list);
            }
        }

        [HttpPost]

        public ActionResult AddExam(string name_exam_vi, string name_exam_ja, string list_question, string isCurrent, string target)
        {
            try
            {
                if (user == null) return View("Login");
                using (var db = new UMC_TESTEntities())
                {
                    var list = JsonConvert.DeserializeObject<List<Question>>(list_question);
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
                    var examDb = db.EXAMs.Add(exam);
                    db.SaveChanges();
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
    }
}