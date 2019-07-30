using AMGHaulier.App.Services;
using AMGHaulier.Common.Helpers;
using AMGHaulier.Common.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AMGHaulier.App.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        private Appointment appoint;

        public AppointmentViewModel()
        {
            appoint = new Appointment();
            AppTitle = "New Appointment";
            CallerId = "AppointmentViewModel";

            InitialiseCommands();
        }

        public AppointmentViewModel(Appointment modelAppointment):this()
        {
            appoint = modelAppointment;

            AppTitle = "Details";
        }

        #region "Properties"

        public string AppTitle { get; set; }

        public virtual int AppointmentId => appoint.AppointmentId;

        public virtual string Summary
        {
            get { return appoint.Summary; }
            set
            {
                if(appoint.Summary != value)
                {
                    appoint.Summary = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual string Location
        {
            get { return appoint.Location; }
            set
            {
                if (appoint.Location != value)
                {
                    appoint.Location = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual DateTime StartDate
        {
            get { return appoint.StartDate; }
            set
            {
                if (appoint.StartDate != value)
                {
                    appoint.StartDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual DateTime EndDate
        {
            get { return appoint.EndDate; }
            set
            {
                if (appoint.EndDate != value)
                {
                    appoint.EndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public virtual DateTime SystemStamp => appoint.SystemStamp;

        public virtual int SummaryMaxLength => 255;
        public virtual int LocationMaxLength => 255;

        #endregion

        #region "Events & Commands"

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        private void InitialiseCommands()
        {
            SaveCommand = new Command(
                execute: () => {
                    Save();
                    if (!IsError)
                        MessagingCenter.Send<object, eMessageToken>(this, CallerId, eMessageToken.Saved);
                },
                canExecute: () => {

                    if (!string.IsNullOrEmpty(Utility.ValidateSummary(this.appoint.Summary))) return false;
                    if (!string.IsNullOrEmpty(Utility.ValidateLocation(this.appoint.Location))) return false;
                    if (!string.IsNullOrEmpty(Utility.ValidateStartAndEndDates(this.appoint.StartDate, this.appoint.EndDate))) return false;

                    return true;
                }
            );

            DeleteCommand = new Command(
                execute: () => {
                    Delete();
                    if(!IsError)
                        MessagingCenter.Send<object, eMessageToken>(this, CallerId, eMessageToken.Deleted);
                },
                canExecute: () => {
                    return !this.appoint.IsNew;
                }
            );

            CancelCommand = new Command(
                execute: () => {
                    MessagingCenter.Send<object, eMessageToken>(this, CallerId, eMessageToken.Cancel);
                },
                canExecute: () => { return true; }
            );
        }

        protected override void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            Message = string.Empty;
            RefreshCommandStatus();
        }

        private void RefreshCommandStatus()
        {
            (SaveCommand as Command).ChangeCanExecute();
            (DeleteCommand as Command).ChangeCanExecute();
            (CancelCommand as Command).ChangeCanExecute();
        }

        #endregion

        #region "Methods"

        public void Save()
        {
            var dep = DependencyService.Get<Common.ServiceContracts.IAppointment>();
            using (AppointmentService service = new AppointmentService(dep))
            {
                Message = service.ValidateAppointment(appoint);
                if (!IsError)
                {
                    if (appoint.IsNew)
                        CreateNewAppointment(service, appoint);
                    else
                        UpdateAppointment(service, appoint);
                }
            }
        }

        public void Delete()
        {
            var dep = DependencyService.Get<Common.ServiceContracts.IAppointment>();
            using (AppointmentService service = new AppointmentService(dep))
            {
                bool b = service.DeleteAppointment(appoint.AppointmentId);
                if (!b)
                    Message = "Failed to delete Appointment";
            }
        }

        private void CreateNewAppointment(AppointmentService service, Appointment model)
        {
            if (service.CreateAppointment(model) < 1)
                Message = "Error recording new Appointment";
        }

        private void UpdateAppointment(AppointmentService service, Appointment model)
        {
            var res = service.UpdateAppointment(model);
            if (res == null || res.AppointmentId == 0)
                Message = "Error recording changes to Appointment";
        }

        public bool IsValid()
        {
            var dep = DependencyService.Get<Common.ServiceContracts.IAppointment>();
            using (AppointmentService service = new AppointmentService(dep))
            {
                Message = service.ValidateAppointment(appoint);
            }

            return IsError;
        }

        #endregion

    }
}
