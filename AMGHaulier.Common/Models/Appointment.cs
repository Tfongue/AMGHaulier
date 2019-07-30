using System;
using System.Collections.Generic;
using System.Text;

namespace AMGHaulier.Common.Models
{
    public class Appointment
    {
        public Appointment()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public int AppointmentId { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime SystemStamp { get; set; }

        public bool IsNew => AppointmentId == 0;
    }
}
