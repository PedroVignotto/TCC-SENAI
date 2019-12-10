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
    class AddEnvironmentViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand AddCommand => new DelegateCommand(ExecuteAddCommand);

        #endregion

        #region Variables 

        private Environment environment;

        public Environment Environment
        {
            get => environment;
            set => SetProperty(ref environment, value);
        }

        private User user;

        public User User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }


        #endregion

        public AddEnvironmentViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Adicionar ambiente";
        }

        #region Methods 
        public override async void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            User = new User();
            Users = new ObservableCollection<User>();
            Environment = new Environment();

            Body = true;

            CurrentUser = UserJson.GetUsuarioJson();
            Icone = IconTheme.IconName("placeholder");

            await LoadAsync();
        }

        private async Task LoadAsync()
        {

            try
            {
                Users.Clear();

                ObservableCollection<User> users = await HeritageAPIService.GetAsyncUsers(CurrentUser.CompanyId);

                foreach (User user in users)
                {
                    if (user.UserLevel == 2)
                        Users.Add(user);
                }

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarAmbiente");
            }
            finally
            {
                IsBusy = false;
            }

        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        private async void ExecuteAddCommand()
        {
            try
            {
                IsBusy = true;

                Load = true;
                Body = false;

                Environment.CompanyId = CurrentUser.CompanyId;

                if (Validation(Environment))
                {
                    Environment.UserId = User.Id;
                    Repost repost = await HeritageAPIService.PostAsync(Environment);

                    if (repost.Success)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Ambiente adicionado com sucesso");
                        Environment = new Environment();
                    }
                    else
                    {
                        DialogParameters param = new DialogParameters
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
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao adicionar ambiente" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarAmbiente");
            }
            finally
            {
                IsBusy = false;
                Load = false;
                Body = true;
            }

        }

        private bool Validation(Environment environment)
        {
            if (environment.Name == null || User.Id == null)
                return false;
            else
                return true;
        }

        #endregion

    }
}
