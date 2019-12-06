using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV04.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {

        #region Variables

        private ObservableCollection<Itens> itens;

        public ObservableCollection<Itens> Itens
        {
            get => itens;
            set => SetProperty(ref itens, value);
        }

        #endregion

        #region Command

        private DelegateCommand<Itens> _ItensCommand;
        public DelegateCommand<Itens> ItensCommand => _ItensCommand ?? (_ItensCommand = new DelegateCommand<Itens>(async (itemSelect) => await ExecuteMenuItensCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion


        public MenuViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDialogService dialogService, IHeritageAPIService heritageAPIService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Menu";

            Theme theme = new Theme();
            theme.ToExchangeTheme();
        }

        #region Métodos

        private async Task ExecuteMenuItensCommand(Itens itens)
        {
            if (itens.Name == "Início" || itens.Name == "Sair")
                await NavigationService.NavigateAsync(new Uri("https://www.Heritage.com/" + itens.Page, UriKind.Absolute));
            else
                await NavigationService.NavigateAsync(new Uri(itens.Page, UriKind.Relative));
        }

        public override void Initialize(INavigationParameters parameters)
        {
            UserJson = new UserJson();
            CurrentUser = new User();
            IconTheme = new IconTheme();
            CurrentUser = UserJson.GetUsuarioJson();

            Itens = new ObservableCollection<Itens>
            {
                new Itens { Name = "Início", Icon = IconTheme.IconName("home"), Page = "Menu/NavigationPage/Main"},
                new Itens { Name = "Perfil", Icon = IconTheme.IconName("avatar"), Page = "NavigationPage/Perfil"},
                new Itens { Name = "Manutenções", Icon = IconTheme.IconName("tools"), Page = "NavigationPage/Manutencoes"},
                new Itens { Name = "Movimentações", Icon = IconTheme.IconName("acceleration"), Page = "NavigationPage/Movimentacoes"},
                new Itens { Name = "Configurações", Icon = IconTheme.IconName("settings"), Page = "NavigationPage/Configuracoes"},
                new Itens { Name = "Sair", Icon = IconTheme.IconName("logout"), Page = "Login"},
            };

        }

        #endregion

    }
}
