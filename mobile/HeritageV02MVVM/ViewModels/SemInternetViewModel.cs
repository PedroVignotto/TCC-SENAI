using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;

namespace HeritageV02MVVM.ViewModels
{
    public class SemInternetViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand); 

        #endregion

        #region Variáveis

        private string _iconPrincipal;

        public string IconPrincipal
        {
            get { return _iconPrincipal; }
            set => SetProperty(ref _iconPrincipal, value);
        } 

        #endregion

        public SemInternetViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Internet";

            Icon = new Icon();
            Icone = Icon.IconName("wifi");
        }

        #region Métodos

        private void ExecuteRefreshCommand()
        {
            NavigationService.NavigateAsync(new Uri("https://www.Heritage.com/Menu/NavigationPage/Main"));
        } 

        #endregion

    }
}
