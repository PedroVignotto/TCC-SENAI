﻿using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {

        #region Variáveis

        private ObservableCollection<MenuItemList> menuItemLists;

        public ObservableCollection<MenuItemList> MenuItemLists
        {
            get => menuItemLists;
            set => SetProperty(ref menuItemLists, value);
        }

        #endregion

        #region Command

        private DelegateCommand<MenuItemList> _MenuItemListCommand;
        public DelegateCommand<MenuItemList> MenuItemLsitCommand => _MenuItemListCommand ?? (_MenuItemListCommand = new DelegateCommand<MenuItemList>(async (itemSelect) => await ExecuteMenuItensCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion


        public MenuViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IDialogService dialogService, IHeritageAPIService heritageAPIService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Menu";

            Theme theme = new Theme();
            theme.ToExchangeTheme();

            MenuItemLists = new ObservableCollection<MenuItemList>
            {
                new MenuItemList { Nome = "Início", Icone = Icon.IconName("home"), Pagina = "Menu/NavigationPage/Main"},
                new MenuItemList { Nome = "Perfil", Icone = Icon.IconName("avatar"), Pagina = "NavigationPage/Perfil"},
                new MenuItemList { Nome = "Manutenções", Icone = Icon.IconName("tools"), Pagina = "NavigationPage/Manutencoes"},
                new MenuItemList { Nome = "Movimentações", Icone = Icon.IconName("acceleration"), Pagina = "NavigationPage/Movimentacoes"},
                new MenuItemList { Nome = "Configurações", Icone = Icon.IconName("settings"), Pagina = "NavigationPage/Configuracoes"},
                new MenuItemList { Nome = "Sair", Icone = Icon.IconName("logout"), Pagina = "Login"},
            };

        }

        #region Métodos

        private async Task ExecuteMenuItensCommand(MenuItemList menuItemList)
        {
            if (menuItemList.Nome == "Início" || menuItemList.Nome == "Sair")
                await NavigationService.NavigateAsync(new Uri("https://www.Heritage.com/" + menuItemList.Pagina, UriKind.Absolute));
            else
                await NavigationService.NavigateAsync(new Uri(menuItemList.Pagina, UriKind.Relative));
        }

        public override async void Initialize(INavigationParameters parameters)
        {

            JsonUsuario = new JsonUsuario();
            UsuarioAtual = new Usuario();
            Icon = new Icon();
            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            
        }

        public override void Destroy()
        {
            MenuItemLists = null;
            Title = null;
            JsonUsuario = null;
            UsuarioAtual = null;
            Icon = null;
            UsuarioAtual = null;
        }

        #endregion

    }
}
