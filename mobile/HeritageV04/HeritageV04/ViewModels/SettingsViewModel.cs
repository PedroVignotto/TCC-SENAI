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
using Xamarin.Forms;

namespace HeritageV04.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand ChangeThemeCommand => new DelegateCommand(ExecuteChangeThemeCommand);

        private DelegateCommand<Itens> _ItensCommand;
        public DelegateCommand<Itens> ItensCommand => _ItensCommand ?? (_ItensCommand = new DelegateCommand<Itens>(async (itemSelect) => await ExecuteMenuItensCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variables

        private ObservableCollection<Itens> itens;

        public ObservableCollection<Itens> Itens
        {
            get => itens;
            set => SetProperty(ref itens, value);
        }

        private string _themeIcon;
        public string ThemeIcon
        {
            get { return _themeIcon; }
            set => SetProperty(ref _themeIcon, value);
        }

        private bool _isToggled;
        public bool IsToggled
        {
            get { return _isToggled; }
            set => SetProperty(ref _isToggled, value);
        }

        #endregion

        public SettingsViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Configurações";
            Icone = "settingsLight.png";

            if (Application.Current.Properties.ContainsKey("TemaActived"))
                IsToggled = Convert.ToBoolean(Application.Current.Properties["TemaActived"]);
            else
            {
                Application.Current.Properties["TemaActived"] = "false";
                IsToggled = false;
            }
        }

        #region Methods

        public override void Initialize(INavigationParameters parameters)
        {
            Itens = new ObservableCollection<Itens>();
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();

            CurrentUser = UserJson.GetUsuarioJson();

            Itens = new ObservableCollection<Itens>
            {
                new Itens { Name = "Perfil", Icon = IconTheme.IconName("avatar"), Page = "Perfil", Description = "Ver e alterar seu perfil" },
                new Itens { Name = "Versão do Heritage", Icon = IconTheme.IconName("smartphone"), Description = "Versão: 0.0.2"},
                new Itens { Name = "Sobre nós", Icon = IconTheme.IconName("question"), Description = "Sobre os desenvolvedores do aplicativo"},
                new Itens { Name = "Funcionamento", Icon = IconTheme.IconName("exclamation"), Description = "Como o aplicativo funciona"},
                new Itens { Name = "Sair", Icon = IconTheme.IconName("logout"), Page = "Inicio", Description = "Sair e deslogar do aplicativo"},
            };

            ThemeIcon = IconTheme.IconName("theme");

        }

        private async Task ExecuteMenuItensCommand(Itens menuItem)
        {
            await NavigationService.NavigateAsync(menuItem.Page);
        }

        private void ExecuteChangeThemeCommand()
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

            IconTheme = new IconTheme();

            Itens[0].Icon = IconTheme.IconName("avatar");
            Itens[1].Icon = IconTheme.IconName("smartphone");
            Itens[2].Icon = IconTheme.IconName("question");
            Itens[3].Icon = IconTheme.IconName("exclamation");
            Itens[4].Icon = IconTheme.IconName("logout");
            ThemeIcon = IconTheme.IconName("theme");
        }

        #endregion

    }
}
