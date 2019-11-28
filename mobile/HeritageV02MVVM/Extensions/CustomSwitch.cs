using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HeritageV02MVVM.Extensions
{
    public class CustomSwitch : Behavior<Switch>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomSwitch), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Switch Bindable { get; private set; }

        protected override void OnAttachedTo(Switch bindable)
        {
            base.OnAttachedTo(bindable);
            Bindable = bindable;
            Bindable.BindingContextChanged += OnBindingContextChanged;
            Bindable.Toggled += OnSwitchToggled;
        }

        protected override void OnDetachingFrom(Switch bindable)
        {
            base.OnDetachingFrom(bindable);
            Bindable.BindingContextChanged -= OnBindingContextChanged;
            Bindable.Toggled -= OnSwitchToggled;
            Bindable = null;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
            BindingContext = Bindable.BindingContext;
        }

        private void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            Command?.Execute(e.Value);
        }
    }
}
