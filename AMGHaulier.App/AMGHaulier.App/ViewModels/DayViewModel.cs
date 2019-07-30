using AMGHaulier.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMGHaulier.App.ViewModels
{
    public class DayViewModel: BaseViewModel
    {

        public DayViewModel(DateTime date, List<Appointment> appointments)
        {
            this.Date = date;
            if (appointments.Count > 0)
                this.Appointments = appointments.ConvertAll(a => new AppointmentViewModel(a));
            else
                this.Appointments = new List<AppointmentViewModel>();
        }

        #region "Properties"


        #endregion

        #region "Methods"

        public List<AppointmentViewModel> Appointments { get; private set; }

        public DateTime Date { get; private set; }

        #endregion

    }
}
