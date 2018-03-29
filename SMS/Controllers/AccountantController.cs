using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace SMS.Controllers
{
    [Authorize(Roles = "accountant")]
    public class AccountantController : Controller
    {
        private Entities db = new Entities();
        private ApplicationDbContext context = new ApplicationDbContext();
        
        // GET: Accountant
        public ActionResult Index()
        {
            return View();
        }

        #region Student


        #endregion

        #region Parent

        public ActionResult CreateParent()
        {
            return View();
        }

        #endregion

        #region Teacher

        public ActionResult ListTeacher()
        {
            return View(db.Teachers);
        }

        public ActionResult CreateTeacher()
        {
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeacher(TeacherEmployeeUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var user = new ApplicationUser
                    {
                        UserName = model.Username
                    };
                    var result = userManager.Create(user, model.Password);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Could not create user");
                    }
                    var employee = new Employee
                    {
                        Address = model.Address,
                        CNIC = model.CNIC,
                        Contact = model.Contact,
                        DesignationId = model.DesignationId,
                        Name = model.Name
                    };
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    var teacher = new Teacher
                    {
                        EmployeeId = employee.Id
                    };
                    db.Teachers.Add(teacher);
                    db.SaveChanges();
                    return RedirectToAction("ListTeacher");
                }
                else
                {
                    throw new Exception("Model state invalid");
                }
            }
            catch
            {
                throw;
            }
        }
     
        #endregion
        #region FinanceReport
        public ActionResult CreateFinanceReport()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFinanceReport(FinanceReport report)
        {
            if (ModelState.IsValid)
            {
                report.Date = DateTime.Now;
                db.FinanceReports.Add(report);
                db.SaveChanges();
                return null;
            }
            else {
                throw new Exception("Model state is invalid");
            }
        }
        public ActionResult ListFinanceReport()
        {
            return View(db.FinanceReports);
        }
        public ActionResult EditFinanceReport(int id)
        {
            var model = db.FinanceReports.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFinanceReport(FinanceReport model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return View(model);
            }
            else
            {
                throw new Exception("model not found");
            }
        }
        public ActionResult DeleteFinanceReport(int id)
        {
            var model = db.FinanceReports.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
          
            db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ListFinanceReport");
        }
        #endregion

        #region AdmissionForm
        public ActionResult AddmissionForm()
        {
            return View();
        }
        #endregion



      
    }
}