using HeritageV02MVVM.Services.Abstraction;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageV02MVVM.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService)
            : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Main Page";
        }
    }
}
