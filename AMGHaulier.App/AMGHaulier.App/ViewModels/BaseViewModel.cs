using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AMGHaulier.App.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private string message;
        
        public string Message
        {
            get { return message; }
            set
            {
                if(message != value)
                {
                    message = value;
                    OnPropertyChanged();
                    OnPropertyChanged("IsError");
                }
            }
        }

        public bool IsError => !string.IsNullOrEmpty(message);

        public string CallerId { get; set; }

        #region "Events"

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

    }
}
