using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;

namespace HeritageV04.ViewModels
{
    class WithoutInternetViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand);

        #endregion

        #region Variables

        private string _iconPrincipal;

        public string IconPrincipal
        {
            get { return _iconPrincipal; }
            set => SetProperty(ref _iconPrincipal, value);
        }

        #endregion

        public WithoutInternetViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Internet";

            IconTheme = new IconTheme();
            Icone = IconTheme.IconName("wifi");
        }

        #region Métodos

        private void ExecuteRefreshCommand()
        {
            NavigationService.NavigateAsync(new Uri("https://www.Heritage.com/Menu/NavigationPage/Main"));
        }

        #endregion


    }
}
