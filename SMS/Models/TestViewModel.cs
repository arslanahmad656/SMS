using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS.Models
{
    public class TestViewModel
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int SubjectId { get; set; }
       [Required]
        public string Type { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
        [Required]
        public decimal TotalMarks { get; set; }
    }
}