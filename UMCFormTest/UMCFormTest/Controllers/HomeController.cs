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
            if (user == null) return View("Login");
            using (var db = new UMC_TESTEntities())
            {
                var exam = db.EXAMs.Include("QUESTIONs").Where(m => m.IsCurrent == true).FirstOrDefault();
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
                        GA_UMCEntities gaDB = new GA_UMCEntities();
                        var staff = gaDB.sp_Get_All_Staff_2().Where(m => m.StaffCode == member.StaffCode).FirstOrDefault();
                        if (staff == null)
                        {
                            ModelState.AddModelError("Error", "Kiểm tra lại Code hoặc Password!");
                            return View("Index");
                        }
                        else
                        {
                            user = new STAFF()
                            {
                                StaffCode = staff.StaffCode,
                                Dept = staff.DeptCode,
                                FullName = staff.FullName,
                                Password = "umcvn"
                            };
                            if (user.Dept == "GA")
                            {
                                user.IsReviewer = true;
                            }
                            db.STAFFs.Add(user);
                            db.SaveChanges();
                            SessionHelper.Set(Constant.SESSION_LOGIN, user);
                            return RedirectToAction("Index", "Home");
                        }

                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Kiểm tra lại Code hoặc Password!");
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
    }
}