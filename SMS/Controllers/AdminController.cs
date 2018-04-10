using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using SMS.Models;
using System.Net;

namespace SMS.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private Entities db = new Entities();
        private ApplicationDbContext context = new ApplicationDbContext();
        //Get Admin
        public ActionResult Index()
        {
            return View();
        }
        #region ViewStudent
        public ActionResult ViewStudent()
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
            int? studentClassId = db.StudentClasses.Where(sc => sc.StudentId == id).FirstOrDefault()?.Id ?? null;
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Title", studentClassId);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EitStudent(Student model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    var classId = Convert.ToInt32(Request.Form["classId"]);
                    StudentClass studentClass = null;
                    try
                    {
                        studentClass = db.StudentClasses.Where(sc => sc.StudentId == model.Id).First();
                        studentClass.ClassId = classId;
                        db.Entry(studentClass).State = EntityState.Modified;
                    }
                    catch
                    {
                        studentClass = new StudentClass
                        {
                            StudentId = model.Id,
                            ClassId = classId,
                        };
                        db.StudentClasses.Add(studentClass);
                    }
                    db.SaveChanges();
                    return RedirectToAction("ListStudent");
                }
                else
                {
                    throw new Exception("Model state is invalid");
                }
            }
            catch
            {
                throw;
            }
        }
        public ActionResult DeleteStudent(int id)
        {
            Student model = db.Students.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteStudent")]
        public ActionResult ConfirmDeleteStudent(int id)
        {
            Student model = db.Students.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            db.Entry(model).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("ListStudent");
        }
        #endregion
        #region viewFinanceReport
        public ActionResult ViewFinanceReport()
        { 
            
            return View();

        }

        #endregion


        #region Teacher
        public ActionResult ListTeacher()
        {
            var teacherIds = db.Teachers.Select(t => t.Id).ToList();

            var employees = db.Employees.Where(e => teacherIds.Contains(e.Id)).ToList();

            return View(employees);
        }
        public ActionResult EditEmployee(int id)
        {
            var teacher = db.Employees.Find(id);
            if (teacher == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Title", teacher.DesignationId);
            return View(teacher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(Employee teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EmployeeList");
                }
                else {
                    throw new Exception("ModelState is inValid");
                }
            }
            catch {
                throw;
            }
        }
        public ActionResult DeleteEmployee(int id)
        {
            var model = db.Employees.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);              
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteEmployee")]
        public ActionResult ConfirmEmployeeDelete(int id)
        {
            var model = db.Employees.Find(id);
            db.Employees.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListTeacher");

        }

        #endregion
        #region message
        public ActionResult CreateMessage()
        {
            return View();
        }
        #endregion
    }



}