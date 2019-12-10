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
using Environment = HeritageV04.Models.Environment;

namespace HeritageV04.ViewModels
{
    public class AddHeritageViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand AddCommand => new DelegateCommand(ExecuteAddCommand);

        #endregion

        #region Variables

        private ObservableCollection<Environment> environments;

        public ObservableCollection<Environment> Environments
        {
            get => environments;
            set => SetProperty(ref environments, value);
        }

        private ObservableCollection<Heritage> heritages;

        public ObservableCollection<Heritage> Heritages
        {
            get { return heritages; }
            set { SetProperty(ref heritages, value); }
        }

        private Heritage heritage;

        public Heritage Heritage
        {
            get => heritage;
            set => SetProperty(ref heritage, value);
        }

        private Environment environment;

        public Environment Environment
        {
            get => environment;
            set => SetProperty(ref environment, value);
        }

        #endregion

        public AddHeritageViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Adicionar patrimônio";
        }

        #region Methods
        public override async void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            Environment = new Environment();
            Environments = new ObservableCollection<Environment>();
            Heritages = new ObservableCollection<Heritage>();
            Heritage = new Heritage();

            Body = true;

            CurrentUser = UserJson.GetUsuarioJson();
            Icone = IconTheme.IconName("box");

            await LoadAsync();
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.Back)
            {
                if (parameters.ContainsKey("codigo"))
                {
                    Heritage.Code = (string)parameters["codigo"];

                    foreach (Heritage heritage in Heritages)
                    {
                        if (heritage.Code == Heritage.Code)
                        {
                            Heritage.Code = null;
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Código de patrimônio já cadastrado");
                            break;
                        }
                    }
                }
                   
            }
        }

        private async Task LoadAsync()
        {
            try
            {
                Environments.Clear();

                ObservableCollection<Environment> environments = await HeritageAPIService.GetAsyncEnvironments(CurrentUser.CompanyId);

                foreach (Environment environment in environments)
                    Environments.Add(environment);

                IsBusy = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambientes" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarPatrimonio");
            }
            finally
            {
                IsBusy = false;
            }

            try
            {
                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(CurrentUser.CompanyId);

                Heritages.Clear();

                foreach (Heritage heritage in heritages)
                    Heritages.Add(heritage);

            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar patrimônios" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Patrimônios");
            }
            finally
            {
                IsBusy = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        private bool Validation(Heritage heritage)
        {
            if (heritage.Name == null || heritage.Code == null || heritage.Description == null || Environment.Id == null)
                return false;
            else
                return true;
        }

        private async void ExecuteAddCommand()
        {
            try
            {
                Body = false;
                Load = true;

                if (Validation(Heritage))
                {
                    Heritage.EnvironmentId = Environment.Id;
                    Heritage.CompanyId = CurrentUser.CompanyId;

                    Repost repost = await HeritageAPIService.PostAsync(Heritage);

                    if (repost.Success)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio adicionado com sucesso");
                        Heritage = new Heritage();
                    }
                    else
                    {
                        var param = new DialogParameters
                        {
                            { "Message", repost.ErrorMessage },
                            { "Title", "Erro" },
                            { "Icon", IconTheme.IconName("bug") }
                        };

                        DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                    }

                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Preencha todos os campos");
            }
            catch (Exception ex)
            {
                Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao adicionar patrimônio");
                Console.WriteLine($"Erro ao adicionar patrimônio: {ex.Message}, Página: AdicionarPatrimonio");
                Load = false;
            }
            finally
            {
                IsBusy = false;
                Body = true;
                Load = false;
            }
        }

        #endregion

    }
}
