using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AMGHaulier.App.Controls
{

    public class FlexView : FlexLayout
    {
        private IEnumerable itemsSource;
        private TapGestureRecognizer tapGesture;

        public FlexView()
        {

            tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                //if (!((s as StackLayout).BindingContext is ChildViewModel)) return;
                SelectedItemChangedEventArgs es = new SelectedItemChangedEventArgs((s as View).BindingContext);
                OnItemSelected(this, es);

            };

        }

        protected override async void OnBindingContextChanged()
        {
            await Task.Yield();

            //CreateContentWithDataTemplate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                itemsSource = ItemsSource;
                CreateContentWithDataTemplate();
            }
        }

        #region "Properties"

        public DataTemplate ItemTemplate { get; set; }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(IEnumerable), default(IEnumerable), BindingMode.TwoWay);
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }

            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        #endregion

        #region "Events"

        public event EventHandler ItemSelected;
        protected virtual void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EventHandler handler = ItemSelected;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region "Methods"

        private void ClearChildren()
        {
            foreach (var c in this.Children)
                c.GestureRecognizers.Remove(tapGesture);

            this.Children.Clear();
        }

        private async void BuildContent()
        {
            await Task.Yield();

            this.ClearChildren();

            if (itemsSource == null) return;

            if (itemsSource.ToString().ToLower().Contains("{binding ")) return;

            if (ItemTemplate != null) return;

            foreach (var o in ItemsSource)
            {
                Label l = new Label { Text = o.ToString() };
                this.Children.Add(l);
                l.GestureRecognizers.Add(tapGesture);
            }
        }

        private async void CreateContentWithDataTemplate()
        {
            await Task.Yield();

            this.ClearChildren();
            if (itemsSource == null || ItemTemplate == null) return;

            foreach (var o in itemsSource)
            {
                View view = ItemTemplate.CreateContent() as View;
                view.BindingContext = o;
                this.Children.Add(view);

                view.GestureRecognizers.Add(tapGesture);
            }
        }

        void Dispose()
        {
            tapGesture = null;
        }

        #endregion
    }

}
