using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
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

        public ActionResult CreateStudent()
        {
            ViewBag.ParentId = new SelectList(db.Parents, "Id", "Name");
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent(Student model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(model);
                    db.SaveChanges();
                    var classId = Convert.ToInt32(Request.Form["ClassId"]);
                    var @class = db.Classes.Find(classId);
                    var studentClass = new StudentClass
                    {
                        ClassId = classId,
                        StudentId = model.Id
                    };
                    db.StudentClasses.Add(studentClass);
                    db.SaveChanges();
                    return RedirectToAction("ListStudent");

                }
                else
                {
                    throw new Exception("Model State Invalid!");
                }
            }
            catch
            {
                throw;
            }
            
        }

        public ActionResult ListStudent()
        {
            return View(db.Students);
        }

        public ActionResult EditStudent(int id)
        {
            var model = db.Students.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.ParentId = new SelectList(db.Parents, "Id", "Name", model.ParentId);
            return View(model);           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(Student model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListStudent");

                }
                else
                {
                    throw new Exception("Model State Invalid!");

                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult DeleteStudent(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Student model = db.Students.Find(id);
                    if (model == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    }
                    db.Entry(model).State = EntityState.Deleted;
                    db.SaveChangesAsync();

                    //code to delete student from parent

                    return RedirectToAction("ListStudent");
                }
                else
                {
                    throw new Exception("Model State Invalid");
                }
            }
            catch
            {
                throw;
            }
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(Employee model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.Add(model);
                    db.SaveChanges();
                    if(db.Designations.Find(model.DesignationId).Title.Equals("teacher", StringComparison.OrdinalIgnoreCase))
                    {
                        var teacher = new Teacher
                        {
                            EmployeeId = model.Id
                        };
                        db.Teachers.Add(teacher);
                        db.SaveChanges();
                    }
                    return RedirectToAction("EmployeeList");
                }
                else
                {
                    throw new Exception("Model state is invalid");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult EditEmployee(int id)
        {
            var model = db.Employees.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            //ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Title", model.DesignationId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(Employee model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EmployeeList");
                }
                else
                {
                    throw new Exception("Model state is invalid");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}