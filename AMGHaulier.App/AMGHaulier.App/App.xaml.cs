using AMGHaulier.App.Services;
using AMGHaulier.Common.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AMGHaulier.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            LoadTestData();

            MainPage = new NavigationPage(new MainPage());
        }

        private void LoadTestData()
        {
            var dep = DependencyService.Get<Common.ServiceContracts.IAppointment>();
            Appointment model = new Appointment
            {
                Summary = "Summary test",
                Location = "Nottingham",
                StartDate = new DateTime(2019, 7, 29, 10, 15, 0),
                EndDate = new DateTime(2019, 7, 29, 12, 0, 0)
            };
            using (AppointmentService service = new AppointmentService(dep))
            {
                int i = service.CreateAppointment(model);

                model = new Appointment
                {
                    Summary = "Summary test 2",
                    Location = "Derby",
                    StartDate = new DateTime(2019, 7, 29, 1, 0, 0),
                    EndDate = new DateTime(2019, 7, 29, 2, 0, 0)
                };
                i = service.CreateAppointment(model);

                model = new Appointment
                {
                    Summary = "Summary test 3",
                    Location = "Derby",
                    StartDate = new DateTime(2019, 7, 16, 1, 0, 0),
                    EndDate = new DateTime(2019, 7, 16, 2, 0, 0)
                };
                i = service.CreateAppointment(model);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
