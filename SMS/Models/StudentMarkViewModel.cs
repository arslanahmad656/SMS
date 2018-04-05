using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class StudentMarkViewModel
    {
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public decimal ObtainedMarks { get; set; }

        public System.DateTime Date { get; set; }
    }
}