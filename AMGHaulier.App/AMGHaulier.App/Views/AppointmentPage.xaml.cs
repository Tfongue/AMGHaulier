using AMGHaulier.App.ViewModels;
using AMGHaulier.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMGHaulier.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentPage : ContentPage
    {
        public string CallerId { get; set; }

        public AppointmentPage()
        {
            InitializeComponent();

            CallerId = this.Id.ToString();
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Unsubscribe<object, eMessageToken>(this, this.Id.ToString() + "_AppointmentPage");
            MessagingCenter.Send<object, eMessageToken>(this, CallerId, eMessageToken.Cancel);

            return base.OnBackButtonPressed();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            RegisterMessageHandlers();
        }

        private void RegisterMessageHandlers()
        {
            string callerId = this.Id.ToString() + "_AppointmentPage";
            MessagingCenter.Unsubscribe<object, eMessageToken>(this, callerId);
            if (!(this.BindingContext is AppointmentViewModel))
                return;

            (this.BindingContext as AppointmentViewModel).CallerId = callerId;

            MessagingCenter.Subscribe<object, eMessageToken>(this, callerId, async (psender, msg) =>
            {
                MessagingCenter.Unsubscribe<object, eMessageToken>(this, callerId);
                await Navigation.PopAsync(true);

                MessagingCenter.Send<object, eMessageToken>(this, CallerId, msg);
            });
        }


    }
}