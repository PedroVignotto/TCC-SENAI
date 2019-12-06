using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;

namespace HeritageV04.ViewModels
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