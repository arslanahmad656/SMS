using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class AttendanceViewModel
    {
        [Required]
        [Display(Name = "Target")]
        public int TargetId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Attendance Status")]
        [Range(1, 3, ErrorMessage = "{0} must be one of the valid states: P, A, L")]
        public int AttendanceStatusCode { get; set; }
    }
}