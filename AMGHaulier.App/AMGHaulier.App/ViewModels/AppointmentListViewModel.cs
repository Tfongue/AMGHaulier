using AMGHaulier.App.Services;
using AMGHaulier.Common.Helpers;
using AMGHaulier.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AMGHaulier.App.ViewModels
{
    public class AppointmentListViewModel : BaseViewModel
    {
        private List<Appointment> appointmentList;
        private List<DayViewModel> days;
        private DateTime currentDate;


        public AppointmentListViewModel()
        {
            currentDate = DateTime.Now;
            InitialiseCommands();
        }


        #region "properties"

        public string DisplayCurrentDate { get { return currentDate.ToString("MMM yyyy"); } }

        public List<Appointment> Appointments
        {
            get
            {
                LoadAppointments();
                return appointmentList;
            }
            private set
            {
                if(appointmentList != value)
                {
                    appointmentList = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<DayViewModel> Days
        {
            get
            {
                GenerateDays();
                return days;
            }
            private set
            {
                if(days != value)
                {
                    days = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region "Commands"

        public ICommand NewCommand { get; private set; }

        public ICommand PreviousMonthCommand { get; private set; }

        public ICommand NextMonthCommand { get; private set; }

        #endregion

        #region "Methods"

        private void InitialiseCommands()
        {
            NewCommand = new Command(
                execute: () => { MessagingCenter.Send<object, eMessageToken>(this, CallerId, eMessageToken.NewAppointment); },
                canExecute: () => { return true; }
                );

            PreviousMonthCommand = new Command(
                execute: () => { ChangeMonth(-1); },
                canExecute: () => { return true; }
                );

            NextMonthCommand = new Command(
                execute: () => { ChangeMonth(1); },
                canExecute: () => { return true; }
                );
        }

        private void ChangeMonth(int month)
        {
            currentDate = currentDate.AddMonths(month);
            
            Reload();
        }

        public void LoadAppointments()
        {
            if(appointmentList == null)
            {
                var dep = DependencyService.Get<Common.ServiceContracts.IAppointment>();
                using (AppointmentService service = new AppointmentService(dep))
                {
                    appointmentList = service.GetAppointmentsByMonth(currentDate.Month);
                }
            }
        }

        private void GenerateDays()
        {
            if (days != null) return;

            days = new List<DayViewModel>();
            int totalDays = new DateTime(currentDate.AddMonths(1).Year, currentDate.AddMonths(1).Month, 1).AddDays(-1).Day;
            DateTime date;
            while (days.Count < totalDays)
            {
                date = new DateTime(currentDate.Year, currentDate.Month, days.Count + 1);
                days.Add(new DayViewModel(date, Appointments.Where(a => a.StartDate.Day == days.Count + 1).ToList()));
            }
        }

        public void Reload()
        {
            Appointments = null;
            OnPropertyChanged("DisplayCurrentDate");
            OnPropertyChanged("Appointments");

            days = null;
            GenerateDays();

            OnPropertyChanged("Days");
        }

        #endregion

    }
}
