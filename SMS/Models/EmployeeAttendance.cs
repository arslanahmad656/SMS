//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class EmployeeAttendance
    {
        public int Id { get; set; }
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
    
        public virtual Attendance Attendance { get; set; }
        public virtual Employee Employee { get; set; }
    }
}