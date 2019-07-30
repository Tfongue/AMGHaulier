using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMGHaulier.Common.Helpers;
using AMGHaulier.Common.Models;
using AMGHaulier.Common.ServiceContracts;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(AMGHaulier.App.Droid.Services.AppointmentRepositoryTest))]

namespace AMGHaulier.App.Droid.Services
{

    public class AppointmentRepositoryTest : IAppointment
    {
        public AppointmentRepositoryTest()
        {

        }

        public string ValidateAppointment(Appointment model)
        {
            throw new NotImplementedException();
        }

        public int CreateAppointment(Appointment model)
        {
            var app = TestDatabase.Add(model);

            return app.AppointmentId;
        }

        public Appointment GetAppointment(int Id)
        {
            return TestDatabase.GetAppointments().Where(a => a.AppointmentId == Id).FirstOrDefault();
        }

        public Appointment UpdateAppointment(Appointment model)
        {
            return TestDatabase.Update(model);
        }

        public bool DeleteAppointment(int Id)
        {
            return TestDatabase.DeleteAppointment(Id);
        }

        public List<Appointment> GetAppointmentsByMonth(int month)
        {
            return TestDatabase.GetAppointments().Where(a => a.StartDate.Month == month || a.EndDate.Month == month).ToList();
        }


        public void Dispose()
        {

        }
    }

    static class TestDatabase
    {
        private static List<Appointment> db;

        private static void Initialise()
        {
            if (db == null)
                db = new List<Appointment>();
        }

        public static Appointment Add(Appointment model)
        {
            Initialise();

            var check = TestDatabase.Get(model.AppointmentId);
            if (check != null)
                throw new Exception("Error inserting record: Appointment.AppointmentId is auto incremented.");

            model.AppointmentId = db.Count + 1;
            model.SystemStamp = DateTime.Now;
            db.Add(model);

            return model;
        }

        public static Appointment Get(DateTime start, DateTime end)
        {
            Initialise();

            if (db.Any(x => x.StartDate.ToString(Utility.DateTimeFormat) == start.ToString(Utility.DateTimeFormat) &&
                 x.EndDate.ToString(Utility.DateTimeFormat) == end.ToString(Utility.DateTimeFormat)))
            {
                return db.Where(x => x.StartDate.ToString(Utility.DateTimeFormat) == start.ToString(Utility.DateTimeFormat) &&
                    x.EndDate.ToString(Utility.DateTimeFormat) == end.ToString(Utility.DateTimeFormat)).FirstOrDefault();
            }

            return null;
        }

        public static Appointment Get(int Id)
        {
            Initialise();

            if (db.Any(x => x.AppointmentId == Id))
            {
                return db.Where(x => x.AppointmentId == Id).FirstOrDefault();
            }

            return null;
        }

        public static List<Appointment> GetAppointments()
        {
            Initialise();

            return db.ToList();
        }

        public static Appointment Update(Appointment model)
        {
            Initialise();

            Appointment check = null;
            if (db.Any(x => x.AppointmentId == model.AppointmentId))
            {
                check = db.Where(x => x.AppointmentId == model.AppointmentId).FirstOrDefault();
            }
            if (check == null || !db.Remove(check)) return null;

            db.Add(model);

            return Get(model.AppointmentId);
        }

        public static bool DeleteAppointment(int Id)
        {
            Initialise();

            Appointment check = null;
            if (db.Any(x => x.AppointmentId == Id))
            {
                check = db.Where(x => x.AppointmentId == Id).FirstOrDefault();
                return !db.Remove(check);
            }

            return false;
        }

    }

}