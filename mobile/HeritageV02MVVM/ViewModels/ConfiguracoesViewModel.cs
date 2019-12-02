using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Themes;
using HeritageV02MVVM.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeritageV02MVVM.ViewModels
{
    public class ConfiguracoesViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand MudarTemaCommand { get; private set; }

        private DelegateCommand<MenuItemList> _MenuItensCommand;
        public DelegateCommand<MenuItemList> MenuItensCommand => _MenuItensCommand ?? (_MenuItensCommand = new DelegateCommand<MenuItemList>(async (itemSelect) => await ExecuteMenuItensCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        private ObservableCollection<MenuItemList> menuItemLists;

        public ObservableCollection<MenuItemList> MenuItemLists
        {
            get => menuItemLists;
            set => SetProperty(ref menuItemLists, value);
        }

        private string _iconTheme;
        public string IconTheme
        {
            get { return _iconTheme; }
            set => SetProperty(ref _iconTheme, value);
        }

        private bool _isToggled;
        public bool IsToggled
        {
            get { return _isToggled; }
            set => SetProperty(ref _isToggled, value);
        }

        #endregion

        public ConfiguracoesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Configurações";
            Icone = "settingsLight.png";

            MudarTemaCommand = new DelegateCommand(ExecuteMudarTemaCommand);

            if (Application.Current.Properties.ContainsKey("TemaActived"))
                IsToggled = Convert.ToBoolean(Application.Current.Properties["TemaActived"]);
            else
            {
                Application.Current.Properties["TemaActived"] = "false";
                IsToggled = false;
            }
        }

        #region Métodos

        public override void Initialize(INavigationParameters parameters)
        {
            Icon = new Icon();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            menuItemLists = new ObservableCollection<MenuItemList>();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            MenuItemLists = new ObservableCollection<MenuItemList>
            {
                new MenuItemList { Nome = "Perfil", Icone = Icon.IconName("avatar"), Pagina = "Perfil", Descricao = "Ver e alterar seu perfil" },
                new MenuItemList { Nome = "Versão do Heritage", Icone = Icon.IconName("smartphone"), Descricao = "Versão: 0.0.2"},
                new MenuItemList { Nome = "Sobre nós", Icone = Icon.IconName("question"), Descricao = "Sobre os desenvolvedores do aplicativo"},
                new MenuItemList { Nome = "Funcionamento", Icone = Icon.IconName("exclamation"), Descricao = "Como o aplicativo funciona"},
                new MenuItemList { Nome = "Sair", Icone = Icon.IconName("logout"), Pagina = "Inicio", Descricao = "Sair e deslogar do aplicativo"},
            };

            IconTheme = Icon.IconName("theme");

        }

        private async Task ExecuteMenuItensCommand(MenuItemList menuItem)
        {
            await NavigationService.NavigateAsync(menuItem.Pagina);
        }

        private void ExecuteMudarTemaCommand()
        {
            string tema;

            if (IsToggled == true)
                tema = "Dark";
            else
                tema = "Light";

            if (Application.Current.Properties.ContainsKey("Theme"))
                Application.Current.Properties["Theme"] = tema;
            else
                Application.Current.Properties.Add("Theme", tema);

            Theme theme = new Theme();
            theme.ToExchangeTheme();

            Icon = new Icon();

            MenuItemLists[0].Icone = Icon.IconName("avatar");
            MenuItemLists[1].Icone = Icon.IconName("smartphone");
            MenuItemLists[2].Icone = Icon.IconName("question");
            MenuItemLists[3].Icone = Icon.IconName("exclamation");
            MenuItemLists[4].Icone = Icon.IconName("logout");
            IconTheme = Icon.IconName("theme");
        }

        #endregion

    }
}
