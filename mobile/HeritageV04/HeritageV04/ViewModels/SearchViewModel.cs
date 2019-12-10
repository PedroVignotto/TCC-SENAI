using HeritageV04.Events;
using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Environment = HeritageV04.Models.Environment;

namespace HeritageV04.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {

        #region Variables
        private ObservableCollection<Heritage> heritages;
        public ObservableCollection<Heritage> Heritages
        {
            get { return heritages; }
            set { SetProperty(ref heritages, value); }
        }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        private ObservableCollection<Environment> environments;
        public ObservableCollection<Environment> Environments
        {
            get { return environments; }
            set { SetProperty(ref environments, value); }
        }

        readonly IEventAggregator _eventAggregator;

        private string search;

        public string Search
        {
            get { return search; }
            set
            {
                if (SetProperty(ref search, value))
                {
                    try
                    {
                        ObservableCollection<Heritage> heritages = new ObservableCollection<Heritage>();
                        foreach (Heritage heritage in Heritages)
                        {
                            string name = heritage.Name.ToLower();
                            string code = heritage.Code.ToLower();
                            string description = heritage.Description.ToLower();
                            string environmentName = heritage.Environment.Name.ToLower();

                            if (name.Contains(Search.ToLower()) || code.Contains(Search.ToLower()) || description.Contains(Search.ToLower()) || environmentName.Contains(Search.ToLower()))
                                heritages.Add(heritage);
                        }
                        _eventAggregator.GetEvent<EventCommunicationHeritages>().Publish(heritages);
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao pesquisar patrimônio");
                    }

                    try
                    {
                        ObservableCollection<Environment> environments = new ObservableCollection<Environment>();
                        foreach (Environment environment in Environments)
                        {
                            string name = environment.Name.ToLower();
                            if (name.Contains(Search.ToLower()))
                                environments.Add(environment);
                        }
                        _eventAggregator.GetEvent<EventCommunicationEnvironments>().Publish(environments);
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao pesquisar ambientes");
                    }

                    try
                    {
                        ObservableCollection<User> users = new ObservableCollection<User>();
                        foreach (User user in Users)
                        {
                            string name = user.Name.ToLower();
                            string email = user.Email.ToLower();
                            if (name.Contains(Search.ToLower()) || email.Contains(Search.ToLower()))
                                users.Add(user);
                        }
                        _eventAggregator.GetEvent<EventCommunicationUsers>().Publish(users);
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao pesquisar usuários");
                    }
                }
            }
        } 
        #endregion

        public SearchViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Pesquisar";
            _eventAggregator = eventAggregator;
        }

        #region Methods

        public override async void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            Environments = new ObservableCollection<Environment>();
            Heritages = new ObservableCollection<Heritage>();
            Users = new ObservableCollection<User>();

            Body = true;

            CurrentUser = UserJson.GetUsuarioJson();

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(CurrentUser.CompanyId);

                Heritages.Clear();

                foreach (Heritage heritage in heritages)
                    Heritages.Add(heritage);

                IsBusy = true;
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

            try
            {
                ObservableCollection<Environment> environments = await HeritageAPIService.GetAsyncEnvironments(CurrentUser.CompanyId);

                Environments.Clear();

                foreach (Environment environment in environments)
                    Environments.Add(environment);

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambientes" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Ambientes");
            }
            finally
            {
                IsBusy = false;
            }

            try
            {
                ObservableCollection<User> users = await HeritageAPIService.GetAsyncUsers(CurrentUser.CompanyId);

                Users.Clear();

                foreach (User user in users)
                {

                    if (user.UserLevel == 1)
                        user.UserLevelDescription = "Administrador";
                    else if (user.UserLevel == 2)
                        user.UserLevelDescription = "Gerenciador";
                    else if (user.UserLevel == 3)
                        user.UserLevelDescription = "Suporte";

                    if (CurrentUser.Id != user.Id)
                        Users.Add(user);
                }
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

                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Usuários");
            }
            finally
            {
                IsBusy = false;
            }

        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        } 
        #endregion

    }
}
