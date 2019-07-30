using AMGHaulier.App.Services;
using AMGHaulier.Common.DAL;
using AMGHaulier.Common.Models;
using AMGHaulier.Common.ServiceContracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AMGHaulier.Test
{
    public class AppointmentTest
    {
        private IAppointment repos;
        private const string inputTooLong = "abcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyzabcdefghijklmnopqrstuvwyz";

        [SetUp]
        public void Setup()
        {
            repos = new AppointmentRepositoryTest();

        }

        [Test]
        public void ValidateAppointmentTest_Summary_Empty()
        {
            Appointment model = new Appointment
            {
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res != val, res != val ? "Passed validation." : res);
        }

        public void ValidateAppointmentTest_Summary_Too_Long()
        {
            Appointment model = new Appointment
            {
                Summary = inputTooLong,
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res != val, res != val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateAppointmentTest_Summary_Correct()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res == val, res == val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateAppointmentTest_Location_Empty()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res != val, res != val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateAppointmentTest_Location_Too_Long()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = inputTooLong,
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res != val, res != val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateAppointmentTest_Location_Correct()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res == val, res == val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateDatesTest_StartDate_Greater_than_EndDate()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 0, 0),
                EndDate = new DateTime(2019, 06, 20, 12, 15, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res != val, res != val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateDatesTest_StartDate_Equal_To_EndDate()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 0, 0),
                EndDate = new DateTime(2019, 7, 29, 10, 0, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res == val, res == val ? "Passed validation." : res);
        }

        [Test]
        public void ValidateDatesTest_Correct_Dates()
        {
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 09, 0, 0),
                EndDate = new DateTime(2019, 7, 29, 11, 15, 0)
            };

            string val = string.Empty;
            string res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.ValidateAppointment(model);
            }

            Assert.IsTrue(res == val, res == val ? "Passed validation." : res);
        }

        [Test]
        [TestCase(3, 0, "No appointments for this month")]
        public void LoadAppointmentsByMonthTest_No_Record(int month, int val, string msg)
        {
            string name = new DateTimeFormatInfo().GetMonthName(month);
            List<Appointment> res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.GetAppointmentsByMonth(month);
            }

            Assert.IsTrue(res.Count == val, msg + ": " + name);
        }

        [Test]
        public void CreateAppointmentTest()
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 1",
                Location = "Nottingham",
                StartDate = new System.DateTime(2019, 07, 29, 10, 15, 0),
                EndDate = new System.DateTime(2019, 07, 29, 12, 0, 0)
            };

            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
            }

            Assert.IsTrue(i > 0, i == 0 ? "Failed to create new Appointment" : "Appointment created ID: " + i.ToString());
        }

        [Test]
        public void LoadAppointmentsByMonthTest()
        {
            Appointment model = new Appointment
            {
                Summary = "Sept appointment 1",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 09, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 09, 29, 12, 0, 0)
            };

            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
            }

            DateTime m = model.StartDate;
            string name = new DateTimeFormatInfo().GetMonthName(m.Month);
            int val = 1;
            List<Appointment> res;
            using (AppointmentService service = new AppointmentService(repos))
            {
                res = service.GetAppointmentsByMonth(m.Month);
            }

            Assert.IsTrue(res.Count == val, "Failed to load Appointments for " + name);
        }

        [Test]
        public void GetAppointmentByIdTest()
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            Appointment val = null;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);
            }

            Assert.IsTrue((val != null && val.AppointmentId == i), (val != null && val.AppointmentId == i) ? "Retrieve Appointment" : "Failed to get Appointment: " + i.ToString());
        }

        [Test]
        public void GetAppointmentByIdTest_Fail()
        {
            Appointment val = null;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                val = service.GetAppointment(i);
            }

            Assert.IsTrue(val == null, val == null ? "Retrieve Appointment" : "Failed to get Appointment: " + i.ToString());
        }

        [Test]
        public void UpdateAppointmentTest_Summary()
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            Appointment val = null;
            string update = model.Summary;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);

                if (val != null && val.AppointmentId == i)
                {
                    val.Summary = "There needs to be some changes around here!";
                    val = service.UpdateAppointment(model);
                }
            }

            Assert.IsTrue((val != null && val.Summary != update), (val != null && val.Summary != update) ? "Appointment updated" : "Update failed: " + i.ToString());
        }

        [Test]
        public void UpdateAppointmentTest_Location()
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            Appointment val = null;
            string update = model.Location;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);

                if (val != null && val.AppointmentId == i)
                {
                    val.Location = "Derby";
                    val = service.UpdateAppointment(model);
                }
            }

            Assert.IsTrue((val != null && val.Location != update), (val != null && val.Location != update) ? "Appointment updated" : "Update failed: " + i.ToString());
        }

        [Test]
        public void UpdateAppointmentTest_StartDate()
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            var date = model.StartDate.AddHours(-1);
            Appointment val = null;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);

                if (val != null && val.AppointmentId == i)
                {
                    val.StartDate = date;
                    val = service.UpdateAppointment(model);
                }
            }

            Assert.IsTrue((val != null && val.StartDate == date), (val != null && val.StartDate == date) ? "Appointment updated" : "Update failed: " + i.ToString());
        }

        [Test]
        public void UpdateAppointmentTest_StartDate_Greater_Than_EndDate()
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            var date = model.StartDate.AddHours(5);
            Appointment val = null;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);

                if (val != null && val.AppointmentId == i)
                {
                    val.StartDate = date;
                    val = service.UpdateAppointment(model);
                }
            }

            Assert.IsTrue((val == null), (val == null) ? "Appointment updated" : "Update failed: " + date.ToString());
        }

        [Test]
        [TestCase(null, null)]
        [TestCase(inputTooLong, null)]
        public void UpdateAppointmentSummaryTest_Fail(string update, Appointment res)
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            Appointment val = null;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);

                if (val != null && val.AppointmentId == i)
                {
                    val.Summary = update;
                    val = service.UpdateAppointment(model);
                }
            }

            Assert.IsTrue(val == res, val != res ? "Appointment updated" : "Update failed: " + i.ToString());
        }

        [Test]
        [TestCase(null, null)]
        [TestCase(inputTooLong, null)]
        public void UpdateAppointmentLocationTest_Fail(string update, Appointment res)
        {
            Appointment model = new Appointment
            {
                Summary = "Test appointment 2",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 08, 2, 14, 45, 0),
                EndDate = new DateTime(2019, 08, 2, 17, 0, 0)
            };

            Appointment val = null;
            int i = 0;
            using (AppointmentService service = new AppointmentService(repos))
            {
                i = service.CreateAppointment(model);
                if (i > 0)
                    val = service.GetAppointment(i);

                if (val != null && val.AppointmentId == i)
                {
                    val.Location = update;
                    val = service.UpdateAppointment(model);
                }
            }

            Assert.IsTrue(val == res, val != res ? "Appointment updated" : "Update failed: " + i.ToString());
        }


    }

}
