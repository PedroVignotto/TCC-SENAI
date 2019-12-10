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
    public class DisplayEnvironmentViewModel : ViewModelBase
    {

        #region Commands
        public DelegateCommand SaveCommand => new DelegateCommand(ExecuteSaveCommand);

        public DelegateCommand DeleteCommand => new DelegateCommand(ExecuteDeleteCommand);

        public DelegateCommand VerificationCommand => new DelegateCommand(ExecuteVerificationCommand);

        private DelegateCommand<Heritage> _DisplayHeritageCommand;
        public DelegateCommand<Heritage> DisplayHeritageCommand => _DisplayHeritageCommand ?? (_DisplayHeritageCommand = new DelegateCommand<Heritage>(async (itemSelect) => await ExecuteDisplayHeritageCommand(itemSelect), (itemSelect) => !IsBusy));
        #endregion

        #region Variables

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        private ObservableCollection<Heritage> heritages;
        public ObservableCollection<Heritage> Heritages
        {
            get { return heritages; }
            set { SetProperty(ref heritages, value); }
        }

        private Environment environment;
        public Environment Environment
        {
            get { return environment; }
            set { SetProperty(ref environment, value); }
        }

        private User user;
        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private string iconHeritage;
        public string IconHeritage
        {
            get { return iconHeritage; }
            set { SetProperty(ref iconHeritage, value); }
        }

        private string iconDelete;
        public string IconDelete
        {
            get => iconDelete;
            set { SetProperty(ref iconDelete, value); }
        }

        private string iconName;
        public string IconName
        {
            get { return iconName; }
            set { SetProperty(ref iconName, value); }
        }

        private string iconUser;
        public string IconUser
        {
            get { return iconUser; }
            set { SetProperty(ref iconUser, value); }
        }

        private string iconVerification;
        public string IconVerification
        {
            get { return iconVerification; }
            set { SetProperty(ref iconVerification, value); }
        }

        private string loadMessage;
        public string LoadMessage
        {
            get { return loadMessage; }
            set { SetProperty(ref loadMessage, value); }
        }

        private int _heightListViewHeritages;

        public int HeightListViewHeritages
        {
            get { return _heightListViewHeritages; }
            set => SetProperty(ref _heightListViewHeritages, value);
        }

        private bool bodyHeritages;

        public bool BodyHeritages
        {
            get { return bodyHeritages; }
            set => SetProperty(ref bodyHeritages, value);
        }

        private bool loadHeritages;

        public bool LoadHeritages
        {
            get { return loadHeritages; }
            set => SetProperty(ref loadHeritages, value);
        }

        private bool nullHeritages;

        public bool NullHeritages
        {
            get { return nullHeritages; }
            set => SetProperty(ref nullHeritages, value);
        }

        #endregion

        public DisplayEnvironmentViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Ambiente";
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            UserJson = new UserJson();
            CurrentUser = new User();
            IconTheme = new IconTheme();
            Heritages = new ObservableCollection<Heritage>();
            Users = new ObservableCollection<User>();
            User = new User();
            Environment = new Environment
            {
                User = new User()
            };

            CurrentUser = UserJson.GetUsuarioJson();

            Icone = IconTheme.IconName("placeholder");
            IconUser = IconTheme.IconName("avatar");
            IconHeritage = IconTheme.IconName("box");
            IconDelete = IconTheme.IconName("cancel");
            IconName = IconTheme.IconName("name");
            IconVerification = IconTheme.IconName("verified");

            if (parameters.ContainsKey("environment"))
                Environment = (Environment)parameters["environment"];

            Body = true;

            await LoadAsync();
        }


        private async void ExecuteSaveCommand()
        {
            try
            {
                if (CurrentUser.UserLevel == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja salvar as alterações?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;

                        LoadMessage = "Atualizando ambiente";
                        Environment.UserId = User.Id;

                        bool up = await HeritageAPIService.PutAsync(Environment);

                        if (up)
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Ambiente atualizado com sucesso");
                        else
                        {
                            DialogParameters param = new DialogParameters
                            {
                                { "Message", "Erro ao alterar ambiente" },
                                { "Title", "Erro" },
                                { "Icon", IconTheme.IconName("bug") }
                            };

                            DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                        }
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a alteração");
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao alterar ambiente" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarAmbiente");
            }
            finally
            {
                Load = false;
                Body = true;
            }
            
        }

        private async Task ExecuteDisplayHeritageCommand(Heritage heritage)
        {
            var navigationParams = new NavigationParameters
            {
                {"heritage", heritage}
            };

            await NavigationService.NavigateAsync("DisplayHeritage", navigationParams);
        }

        private async void ExecuteDeleteCommand()
        {
            try
            {
                if (CurrentUser.UserLevel == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir o ambiente?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Deletando ambientes";

                        bool delete = await HeritageAPIService.DeleteAsync(Environment);

                        if (delete)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Ambiente excluido com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                        {
                            DialogParameters param = new DialogParameters
                            {
                                { "Message", "Erro ao excluir ambiente" },
                                { "Title", "Erro" },
                                { "Icon", IconTheme.IconName("bug") }
                            };

                            DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                        }
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a exclusão");
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao excluir ambiente" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarAmbiente");
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

        private async Task LoadAsync()
        {
            LoadHeritages = true;
            BodyHeritages = false;

            try
            {
                IsBusy = true;

                ObservableCollection<User> users = await HeritageAPIService.GetAsyncUsers(CurrentUser.CompanyId);

                Users.Clear();

                foreach (User user in users)
                {
                    if (user.Id == Environment.UserId)
                        User = user;

                    if (user.UserLevel == 2)
                        Users.Add(user);
                }

            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                throw ex;
            }

            try
            {
                IsBusy = true;

                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(Environment.Id, CurrentUser.CompanyId);

                foreach (Heritage heritage in heritages)
                {
                    Heritages.Add(heritage);
                }

                if (Heritages.Count > 0)
                {
                    HeightListViewHeritages = Heritages.Count * 150;
                    BodyHeritages = true;
                }
                else
                    NullHeritages = true;

            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar patrimônios" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                NullHeritages = true;
                throw ex;
            }
            finally
            {
                IsBusy = false;
                LoadHeritages = false;
            }

            foreach (User user in users)
            {
                if (user.Id == Environment.UserId)
                {
                    User = user;
                    break;
                }
            }
        }

        private async void ExecuteVerificationCommand()
        {
            var navigationParams = new NavigationParameters
            {
                {"heritages", Heritages}
            };

            await HeritageAPIService.VerificationAsync(Environment.Id);

            foreach (Heritage heritage in Heritages)
                heritage.State = false;

            await NavigationService.NavigateAsync("SelectedHeritages", navigationParams);
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

    }
}
