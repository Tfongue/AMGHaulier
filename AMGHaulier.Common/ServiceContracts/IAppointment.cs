using AMGHaulier.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMGHaulier.Common.ServiceContracts
{
    public interface IAppointment : IDisposable
    {

        string ValidateAppointment(Appointment model);
        int CreateAppointment(Appointment model);
        Appointment GetAppointment(int Id);
        Appointment UpdateAppointment(Appointment model);
        bool DeleteAppointment(int Id);
        List<Appointment> GetAppointmentsByMonth(int month);
    }
}
