using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;

namespace HeritageV02MVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Theme theme = new Theme();
            theme.ToExchangeTheme();
        }

    }
}
