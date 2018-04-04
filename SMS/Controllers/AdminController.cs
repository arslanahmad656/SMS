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

        public ActionResult ViewFinanceReport()
        { 
            
            return View();

        }
    }


}