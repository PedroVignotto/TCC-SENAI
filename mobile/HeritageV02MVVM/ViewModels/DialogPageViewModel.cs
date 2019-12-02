using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace HeritageV02MVVM.ViewModels
{
    class DialogPageViewModel : BindableBase, IDialogAware, IAutoInitialize
    {
        private string _message;

        [AutoInitialize(true)]
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _title;

        [AutoInitialize(true)]
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _icon;

        [AutoInitialize(true)]
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public DelegateCommand CloseCommand { get; }

        public DialogPageViewModel()
        {
            CloseCommand = new DelegateCommand(() => RequestClose(null));
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
