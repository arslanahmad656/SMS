using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

        public ActionResult Something()
        {
            var s = new DummyModel
            {
                Age = 30,
                Name = "Someone"
            };
            //return View(new DummyModel
            //{
            //    Age = 30,
            //    Name = "Someone"
            //});
            ViewBag.Age = s.Age;
            ViewBag.Name = s.Name;
            return View();
        }
    }
}