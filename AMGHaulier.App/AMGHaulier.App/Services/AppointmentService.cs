using AMGHaulier.Common.Helpers;
using AMGHaulier.Common.Models;
using AMGHaulier.Common.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AMGHaulier.App.Services
{
    public class AppointmentService : IAppointment
    {
        private IAppointment repos;

        public AppointmentService(IAppointment repository)
        {
            repos = repository;
        }


        public int CreateAppointment(Appointment app)
        {
            if (!string.IsNullOrEmpty(ValidateAppointment(app)))
                return 0;

            return repos.CreateAppointment(app);
        }

        public Appointment GetAppointment(int Id)
        {
            return repos.GetAppointment(Id);
        }

        public List<Appointment> GetAppointmentsByMonth(int month)
        {
            return repos.GetAppointmentsByMonth(month);
        }

        public Appointment UpdateAppointment(Appointment model)
        {
            if (!string.IsNullOrEmpty(ValidateAppointment(model)))
                return null;

            return repos.UpdateAppointment(model);
        }

        public bool DeleteAppointment(int Id)
        {
            return repos.DeleteAppointment(Id);
        }

        public string ValidateAppointment(Appointment model)
        {
            if (model == null) return "Invalid object";

            string val = Utility.ValidateSummary(model.Summary);
            if (!string.IsNullOrEmpty(val))
                return val;

            val = Utility.ValidateLocation(model.Location);
            if (!string.IsNullOrEmpty(val))
                return val;

            val = Utility.ValidateStartAndEndDates(model.StartDate, model.EndDate);

            return val;
        }


        public void Dispose()
        {

        }

    }
}
