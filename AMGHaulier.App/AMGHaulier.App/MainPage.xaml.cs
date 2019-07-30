using AMGHaulier.App.ViewModels;
using AMGHaulier.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AMGHaulier.App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            await Task.Yield();

            base.OnAppearing();

            if (this.BindingContext == null)
                LoadAppointments();
        }

        private void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null || !(e.SelectedItem is AppointmentViewModel)) return;

            ShowAppointmentDetails(e.SelectedItem as AppointmentViewModel);

            (sender as ListView).SelectedItem = null;
        }

        private async void ShowAppointmentDetails(AppointmentViewModel view)
        {
            string callerId = this.Id.ToString() + "_AppointmentPage";
            MessagingCenter.Unsubscribe<object, eMessageToken>(this, callerId);
            MessagingCenter.Subscribe<object, eMessageToken>(this, callerId, async (psender, msg) =>
            {
                await Task.Yield();

                MessagingCenter.Unsubscribe<object, eMessageToken>(this, this.Id.ToString() + "_AppointmentPage");
                if (msg!= eMessageToken.Cancel)
                    (this.BindingContext as AppointmentListViewModel).Reload();
            });

            Page page = new Views.AppointmentPage { BindingContext = view, CallerId = callerId };
            await Navigation.PushAsync(page, true);
        }

        private void NewAppointmentListener()
        {
            string callerId = this.Id.ToString() + "_MainPageNewAppointmentListener";
            MessagingCenter.Unsubscribe<object, eMessageToken>(this, callerId);
            MessagingCenter.Subscribe<object, eMessageToken>(this, callerId, (psender, msg) =>
            {   
                if (msg == eMessageToken.NewAppointment)
                    ShowAppointmentDetails(new AppointmentViewModel());
            });

            (this.BindingContext as AppointmentListViewModel).CallerId = callerId;
        }

        private async void LoadAppointments()
        {
            await Task.Yield();

            this.BindingContext = new AppointmentListViewModel();
            NewAppointmentListener();
        }

    }
}
