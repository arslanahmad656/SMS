﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<AttendanceStatu> AttendanceStatus { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual DbSet<FeeChallan> FeeChallans { get; set; }
        public virtual DbSet<FinanceReport> FinanceReports { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageClass> MessageClasses { get; set; }
        public virtual DbSet<MessageEmployee> MessageEmployees { get; set; }
        public virtual DbSet<MessageStudent> MessageStudents { get; set; }
        public virtual DbSet<Parent> Parents { get; set; }
        public virtual DbSet<PaySlip> PaySlips { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentAttendance> StudentAttendances { get; set; }
        public virtual DbSet<StudentClass> StudentClasses { get; set; }
        public virtual DbSet<StudentTest> StudentTests { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<TeacherSubjectClass> TeacherSubjectClasses { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
    
        public virtual int CreateAttendanceStudent(Nullable<int> studentId, Nullable<System.DateTime> date, Nullable<int> attendanceStatusId)
        {
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("StudentId", studentId) :
                new ObjectParameter("StudentId", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            var attendanceStatusIdParameter = attendanceStatusId.HasValue ?
                new ObjectParameter("AttendanceStatusId", attendanceStatusId) :
                new ObjectParameter("AttendanceStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateAttendanceStudent", studentIdParameter, dateParameter, attendanceStatusIdParameter);
        }
    
        public virtual int CreateAttendanceStudentWithChecks(Nullable<int> studentId, Nullable<System.DateTime> date, Nullable<int> attendanceStatusId)
        {
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("StudentId", studentId) :
                new ObjectParameter("StudentId", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            var attendanceStatusIdParameter = attendanceStatusId.HasValue ?
                new ObjectParameter("AttendanceStatusId", attendanceStatusId) :
                new ObjectParameter("AttendanceStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateAttendanceStudentWithChecks", studentIdParameter, dateParameter, attendanceStatusIdParameter);
        }
    
        public virtual int EditAttendanceStudent(Nullable<int> studentId, Nullable<System.DateTime> date, Nullable<int> attendanceStatusId)
        {
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("StudentId", studentId) :
                new ObjectParameter("StudentId", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));
    
            var attendanceStatusIdParameter = attendanceStatusId.HasValue ?
                new ObjectParameter("AttendanceStatusId", attendanceStatusId) :
                new ObjectParameter("AttendanceStatusId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EditAttendanceStudent", studentIdParameter, dateParameter, attendanceStatusIdParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> SampleProc(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("SampleProc", idParameter);
        }
    }
}
