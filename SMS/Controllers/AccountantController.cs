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
using Rotativa;
using Rotativa.MVC;
using SMS.App_Start;

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
            int? studentClassId = db.StudentClasses.Where(sc => sc.StudentId == id).FirstOrDefault()?.Id ?? null;
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Title", studentClassId);
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
                    var classId = Convert.ToInt32(Request.Form["ClassId"]);
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
                            ClassId = classId
                        };
                        db.StudentClasses.Add(studentClass);
                    }
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
        public ActionResult DeleteStudentConfirmed(int id)
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

        #region Parent

        public ActionResult CreateParent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParent(ParentUserVIewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var UserName = model.Username;
                    var Password = model.Password;
                    var user = new ApplicationUser
                    {
                        UserName = UserName
                    };
                    var result = userManager.Create(user, Password);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Could not create user for parent");
                    }
                    Parent parent = new Parent();
                    parent.UserId = user.Id;
                    parent.Name = model.Name;
                    parent.CNIC = model.CNIC;
                    parent.Contact = model.Contact;

                    db.Parents.Add(parent);
                    db.SaveChanges();

                    return RedirectToAction("ListParent");

                }
                throw new Exception("Model State Invalid in Create Parent");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ListParent()
        {
            return View(db.Parents);
        }

        public ActionResult EditParent(int id)
        {
            var model = db.Parents.Find(id);
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            //ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Title", model.DesignationId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParent (Parent model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListParent");
                }
                throw new Exception("Model State invalid in Edit Parent");

            }
            catch
            {
                throw;
            }
        }

        public ActionResult DeleteParent(int id)
        {
            var model = db.Parents.Find(id);
            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteParent")]
        public ActionResult DeleteParentConfirmed(int id)
        {
            var model = db.Parents.Find(id);
            var userId = model.UserId;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = context.Users.Find(userId);
            var result = userManager.Delete(user);
            if (!result.Succeeded)
            {
                throw new Exception("Could not delete the parent");
            }
            //db.Parents.Remove(model);
            //db.SaveChanges();
            return RedirectToAction("ListParent");
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

        #region Employee

        public ActionResult EmployeeList()
        {
            return View(db.Employees);
        }

        public ActionResult CreateEmployee()
        {
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Title");
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
            ViewBag.DesignationId = new SelectList(db.Designations, "Id", "Title", model.DesignationId);
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

        public ActionResult DeleteEmployee(int id)
        {
            var model = db.Employees.Find(id);
            if(model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(model);
        }

        public ActionResult EmployeeDetails(int id)
        {
            var model = db.Employees.Find(id);
            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteEmployee")]
        public ActionResult DeleteEmployeeConfirmed(int id)
        {
            var model = db.Employees.Find(id);
            db.Employees.Remove(model);
            db.SaveChanges();
            return RedirectToAction("EmployeeList");
        }
        #endregion

        #region Printing

        public ActionResult PrintAdmissionForm()
        {
            var result = new PartialViewAsPdf("AdmissionForm");
            return result;
        }


        public ActionResult PrintFeeChallan(int id)
        {
            var model = db.Students.Find(id);
            var result = new PartialViewAsPdf("PrintFeeChallan",model);
            return result;
           
        }

        public ActionResult PrintPayVoucher(int id)
        {
            var model = db.Employees.Find(id);
            var result = new PartialViewAsPdf("PrintPayVoucher", model);
            return result;

        }


        #endregion

        #region test

        public ActionResult CreateTest()
        {

            //ViewBag.TeacherId = new SelectList(db.Employees, "Id", "Name");

            //var teachers = db.Teachers.Joidb.Employees, (ti => ti.EmployeeId), (ei => ei.Id),((ti, ei)=> new {Teachers=ti, Employees=ei }));
            var teacher = db.Teachers.ToList();
            List<TeacherNameViewModel> teacherNames = new List<TeacherNameViewModel>();
            foreach (Teacher i in teacher)
            {
                TeacherNameViewModel temp= new TeacherNameViewModel();
                temp.Id = i.Id;
                temp.Name = i.Employee.Name;
                teacherNames.Add(temp);            
            
            };
            ViewBag.TeacherId = new SelectList(teacherNames, "Id", "Name");
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Title");
            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "TItle");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTest (TestViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    

                    Test test = new Test();
                    TeacherSubjectClass temp = db.TeacherSubjectClasses.Where(st => st.SubjectId == model.SubjectId && st.TeacherId == model.TeacherId && st.ClassId == model.ClassId && st.IsActive==true).SingleOrDefault();
                    test.TeacherSubjectClassId = temp.Id;
                    test.Date = model.Date;
                    //test.Date = DateTime.Now;
                    test.Type = model.Type;
                    test.TotalMarks = model.TotalMarks;
                    db.Tests.Add(test);
                    db.SaveChanges();
                    return RedirectToAction("ListTest");              
                    
                    
                }
                throw new Exception ("Error in Creating test") ;
            }
            catch
            {
                throw;
            }

        }
        public ActionResult ListTest()
        {
            return View(db.Tests);
        }
        //public ActionResult EditTest(int id)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid) {
        //            Test test = db.Tests.Find(id);
        //            var teacher = db.Teachers.ToList();
        //            List<TeacherNameViewModel> teacherNames = new List<TeacherNameViewModel>();
        //            foreach (Teacher i in teacher)
        //            {
        //                TeacherNameViewModel temp = new TeacherNameViewModel();
        //                temp.Id = i.Id;
        //                temp.Name = i.Employee.Name;
        //                teacherNames.Add(temp);

        //            };
        //            ViewBag.TeacherId = new SelectList(teacherNames, "Id", "Name");
        //            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Title");
        //            ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "TItle");

        //            return View(test);
        //        }
        //        throw new Exception("Model State Invalid in editting a test");
        //    }
        //    catch
        //    {
        //        throw new Exception("Could not edit test");
        //    }

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditTest (Test model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            Test test = db.Tests.Find(model.Id);
        //            if (test == null)
        //            {
        //                throw new Exception("Could not find the test");
        //            }
        //            test.Date = model.Date;
        //            test.TotalMarks = model.TotalMarks;
        //            test.Type=model.Type;
        //            var SubjectId = Int32.Parse(Request.Form["SubjectId"]);
        //            var TeacherId = Int32.Parse(Request.Form["TeacherId"]);
        //            var ClassId = Int32.Parse(Request.Form["ClassId"]);

        //TeacherSubjectClass temp = db.TeacherSubjectClasses.Where(st => st.SubjectId == SubjectId && st.TeacherId == TeacherId && st.ClassId == ClassId && st.IsActive == true).SingleOrDefault();
        //            if (temp == null)
        //            {
        //                throw new Exception("Could not fint TeacheSubjectClass");
        //            }
        //            test.TeacherSubjectClassId = temp.Id;

        //            db.Entry(test).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("ListTest");
        //        }
        //        throw new Exception("ModelState Invalid in Edit Test");

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public ActionResult DeleteTest(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Tests.Find(id);
                    if (model == null)
                    {
                        throw new Exception("Could not find the test");
                    }
                    ViewBag.ClassName = model.TeacherSubjectClass.Class.Title;
                    ViewBag.TeacherName = model.TeacherSubjectClass.Teacher.Employee.Name;
                    ViewBag.SubjectName = model.TeacherSubjectClass.Subject.Title;
                    return View(model);
                }
                throw new Exception("Could not delete test");
            }
            catch
            {
                throw;
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTest(Test model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Deleted;
                    db.SaveChanges();
                    return RedirectToAction("ListTest");
                }
                throw new Exception("Could not delete test");
            }
            catch
            {
                throw;
            }
        }
        public ActionResult MarkStudentMarks()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var teacher = db.Teachers.ToList();
                    List<TeacherNameViewModel> teacherNames = new List<TeacherNameViewModel>();
                    foreach (Teacher i in teacher)
                    {
                        TeacherNameViewModel temp = new TeacherNameViewModel();
                        temp.Id = i.Id;
                        temp.Name = i.Employee.Name;
                        teacherNames.Add(temp);

                    };
                    ViewBag.TeacherId = new SelectList(teacherNames, "Id", "Name");
                    ViewBag.ClassId = new SelectList(db.Classes, "Id", "Title");
                    ViewBag.SubjectId = new SelectList(db.Subjects, "Id", "Title");
                    ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
                    
                    return View();
                }
                throw new Exception("Could not enter marks");
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkStudentMarks(StudentMarkViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    TeacherSubjectClass temp = db.TeacherSubjectClasses.Where(st => st.SubjectId == model.SubjectId && st.TeacherId == model.TeacherId && st.ClassId == model.ClassId && st.IsActive == true).SingleOrDefault();
                    if (temp == null)
                    {
                        throw new Exception("Could not find the TeacherSubjectClass");
                    }
                    Test test = db.Tests.Where(st => st.TeacherSubjectClassId == temp.Id && st.Date == model.Date).SingleOrDefault();
                    if (test == null)
                    {
                        throw new Exception("Could not find the test");
                    }
                    if (model.ObtainedMarks > test.TotalMarks)
                    {
                        throw new Exception("Obtained Marks more than total marks");
                    }
                    StudentTest studentTest = new StudentTest();
                    studentTest.TestId = test.Id;
                    studentTest.StudentId = model.StudentId;
                    studentTest.ObtainedMarks = model.ObtainedMarks;
                    
                    db.StudentTests.Add(studentTest);
                    db.SaveChanges();
                    return RedirectToAction("ListStudentMarks",new {id=test.Id});

                }
                throw new Exception("Could not mark student marks");
            }
            catch
            {
                throw;
            }
        }
        public ActionResult EditStudentMarks(int studentId,int testId)
        {
            try
            {
                StudentTest model = db.StudentTests.Where(st => st.StudentId == studentId && st.TestId == testId).SingleOrDefault();
                return View(model);
            }
            catch 
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentMarks(StudentTest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListStudentMarks", new { id = model.TestId });
                }
                throw new Exception("Could not edit student marks");
            }
            catch 
            {

                throw;
            }
        }
        public ActionResult ListStudentMarks(int id)
        {

            return View(db.StudentTests.Where(st=> st.TestId==id));
        }
        #endregion

        #region AttendanceStudent
        public ActionResult StudentAttendanceList()
        {
            var studentAttendance = db.StudentAttendances.Select(sa => new AttendanceViewModel
            {
                AttendanceStatusCode = sa.Attendance.AttendanceStatu.code,
                Date = sa.Attendance.Date,
                TargetId = sa.StudentId
            }).ToList();
            var studentNameDictionary = new Dictionary<int, string>();
            studentAttendance.ForEach(sa =>
            {
                try
                {
                    studentNameDictionary.Add(sa.TargetId, db.Students.Find(sa.TargetId).Name);
                }
                catch (ArgumentException)
                {

                }
            });
            ViewBag.StudentNameDictionary = studentNameDictionary;
            return View(studentAttendance);
        }

        public ActionResult MarkStudentAttendance()
        {
            ViewBag.AttendanceStatusCode = new SelectList(db.AttendanceStatus, "Code", "Title");
            ViewBag.TargetId = new SelectList(db.Students, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkStudentAttendance(AttendanceViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //var attendance = new Attendance
                    //{
                    //    AttendanceStatusId = db.AttendanceStatus.Where(a => a.code == model.AttendanceStatusCode).First().Id,
                    //    Date = model.Date
                    //};
                    //db.Attendances.Add(attendance);
                    //db.SaveChanges();

                    //var studentAttendance = new StudentAttendance
                    //{
                    //    StudentId = model.TargetId,
                    //    AttendanceId = attendance.Id
                    //};
                    //db.StudentAttendances.Add(studentAttendance);
                    //db.SaveChanges();
                    var res = db.CreateAttendanceStudentWithChecks(model.TargetId, model.Date, db.AttendanceStatus.Where(a => a.code == model.AttendanceStatusCode).First().Id);
                    if(res == -1)
                    {
                        throw new Exception("Cannot insert duplicate attendance");
                    }
                    return RedirectToAction("StudentAttendanceList");
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

        public ActionResult EditStudentAttendance(int studentId, DateTime date)
        {
            var model = db.StudentAttendances.Where(sa => sa.StudentId == studentId && sa.Attendance.Date == date).Select(sa => new AttendanceViewModel
            {
                Date = sa.Attendance.Date,
                TargetId = sa.StudentId,
                AttendanceStatusCode = sa.Attendance.AttendanceStatu.code
            }).FirstOrDefault();
            if(model == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttendanceStatusCode = new SelectList(db.AttendanceStatus, "Code", "Title", model.AttendanceStatusCode);
            ViewBag.TargetId = new SelectList(db.Students, "Id", "Name", model.TargetId);
            ViewBag.StudentName = db.Students.Find(model.TargetId).Name;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentAttendance(AttendanceViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //var studentId = model.TargetId;
                    //var date = model.Date;
                    //var attendanceStatusId = db.AttendanceStatus.Where(at => at.code == model.AttendanceStatusCode).First().Id;
                    //var res = db.EditAttendanceStudent(studentId, date, attendanceStatusId);
                    //if(res != 0)
                    //{
                    //    throw new Exception("There was an error in the model state (sp)");
                    //}
                    

                    var toEdit = db.StudentAttendances.Where(sa => sa.StudentId == model.TargetId && sa.Attendance.Date == model.Date).Select(sa => sa.Attendance).First();
                    toEdit.AttendanceStatusId = db.AttendanceStatus.Where(at => at.code == model.AttendanceStatusCode).First().Id;
                    db.Entry(toEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("StudentAttendanceList");
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

        #endregion

        #region Others

        public ActionResult ExecuteSampleProcedure(int id)
        {
            var res = db.SampleProc(id).FirstOrDefault();
            ViewBag.Result = res;
            return View();
        }

        #endregion

        #region AttendanceStudent
        public ActionResult EmployeeAttendanceList()
        {
            var employeeAttendance = db.EmployeeAttendances.Select(ea => new AttendanceViewModel
            {
                AttendanceStatusCode = ea.Attendance.AttendanceStatu.code,
                Date = ea.Attendance.Date,
                TargetId = ea.EmployeeId
            }).ToList();
            var employeeNameDictionary = new Dictionary<int, string>();
            employeeAttendance.ForEach(ea =>
            {
                try
                {
                    employeeNameDictionary.Add(ea.TargetId, db.Employees.Find(ea.TargetId).Name);
                }
                catch (ArgumentException)
                {

                }
            });
            ViewBag.EmployeeNameDictionary = employeeNameDictionary;
            return View(employeeAttendance);
        }

        public ActionResult MarkEmployeeAttendance()
        {
            ViewBag.AttendanceStatusCode = new SelectList(db.AttendanceStatus, "Code", "Title");
            ViewBag.TargetId = new SelectList(db.Employees, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkEmployeeAttendance(AttendanceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var attendance = new Attendance
                    //{
                    //    AttendanceStatusId = db.AttendanceStatus.Where(a => a.code == model.AttendanceStatusCode).First().Id,
                    //    Date = model.Date
                    //};
                    //db.Attendances.Add(attendance);
                    //db.SaveChanges();

                    //var studentAttendance = new StudentAttendance
                    //{
                    //    StudentId = model.TargetId,
                    //    AttendanceId = attendance.Id
                    //};
                    //db.StudentAttendances.Add(studentAttendance);
                    //db.SaveChanges();
                    var res = db.CreateAttendanceEmployee(model.TargetId, model.Date, db.AttendanceStatus.Where(a => a.code == model.AttendanceStatusCode).First().Id);
                    if (res == -1)
                    {
                        throw new Exception("Cannot insert duplicate attendance");
                    }
                    return RedirectToAction("EmployeeAttendanceList");
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

        public ActionResult EditEmployeeAttendance(int employeeId, DateTime date)
        {
            var model = db.EmployeeAttendances.Where(ea => ea.EmployeeId == employeeId && ea.Attendance.Date == date).Select(sa => new AttendanceViewModel
            {
                Date = sa.Attendance.Date,
                TargetId = sa.EmployeeId,
                AttendanceStatusCode = sa.Attendance.AttendanceStatu.code
            }).FirstOrDefault();
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttendanceStatusCode = new SelectList(db.AttendanceStatus, "Code", "Title", model.AttendanceStatusCode);
            ViewBag.TargetId = new SelectList(db.Employees, "Id", "Name", model.TargetId);
            ViewBag.EmployeeName = db.Employees.Find(model.TargetId).Name;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployeeAttendance(AttendanceViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var studentId = model.TargetId;
                    //var date = model.Date;
                    //var attendanceStatusId = db.AttendanceStatus.Where(at => at.code == model.AttendanceStatusCode).First().Id;
                    //var res = db.EditAttendanceStudent(studentId, date, attendanceStatusId);
                    //if(res != 0)
                    //{
                    //    throw new Exception("There was an error in the model state (sp)");
                    //}


                    var toEdit = db.EmployeeAttendances.Where(ea => ea.EmployeeId == model.TargetId && ea.Attendance.Date == model.Date).Select(sa => sa.Attendance).First();
                    toEdit.AttendanceStatusId = db.AttendanceStatus.Where(at => at.code == model.AttendanceStatusCode).First().Id;
                    db.Entry(toEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EmployeeAttendanceList");
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

        #endregion

    }
}
