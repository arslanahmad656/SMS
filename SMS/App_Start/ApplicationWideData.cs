using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.App_Start
{
    public static class ApplicationWideData
    {
        public static int AttendanceStatusPresent => 1;

        public static int AttendanceStatusAbsent => 2;

        public static int AttendanceStatusLeave => 3;

        public static string GetAttendaceStatusString(int statusCode)
        {
            return statusCode == AttendanceStatusPresent ? "P" : statusCode == AttendanceStatusAbsent ? "A" : statusCode == AttendanceStatusLeave ? "L" : null;
        }
    }
}