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
    public class DisplayHeritageViewModel : ViewModelBase
    {

        #region Commands
        public DelegateCommand SaveCommand => new DelegateCommand(ExecuteSaveCommand);

        public DelegateCommand DeleteCommand => new DelegateCommand(ExecuteDeleteCommand);

        public DelegateCommand AddCommand => new DelegateCommand(ExecuteAddCommand);
        #endregion

        #region Variables
        private Heritage heritage;
        public Heritage Heritage
        {
            get { return heritage; }
            set { SetProperty(ref heritage, value); }
        }

        private ObservableCollection<Environment> environments;
        public ObservableCollection<Environment> Environments
        {
            get { return environments; }
            set { SetProperty(ref environments, value); }
        }

        private Environment environment;
        public Environment Environment
        {
            get { return environment; }
            set 
            {
                SetProperty(ref environment, value);
            }
        }

        public int? EnvironmentId { get; set; }

        private string iconName;
        public string IconName
        {
            get { return iconName; }
            set { SetProperty(ref iconName, value); }
        }

        private string iconCode;
        public string IconCode
        {
            get { return iconCode; }
            set { SetProperty(ref iconCode, value); }
        }

        private string iconEnvironment;
        public string IconEnvironment
        {
            get { return iconEnvironment; }
            set { SetProperty(ref iconEnvironment, value); }
        }

        private string iconState;
        public string IconState
        {
            get { return iconState; }
            set { SetProperty(ref iconState, value); }
        }

        private string iconDescription;
        public string IconDescription
        {
            get => iconDescription;
            set { SetProperty(ref iconDescription, value); }
        }

        private string iconDelete;
        public string IconDelete
        {
            get => iconDelete;
            set { SetProperty(ref iconDelete, value); }
        }

        private string _loadMessage;
        public string LoadMessage
        {
            get => _loadMessage;
            set { SetProperty(ref _loadMessage, value); }
        }

        #endregion

        public DisplayHeritageViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Patrimônio";
        }

        #region Methods

        public override async void Initialize(INavigationParameters parameters)
        {
            Environments = new ObservableCollection<Environment>();
            UserJson = new UserJson();
            CurrentUser = new User();
            IconTheme = new IconTheme();
            Heritage = new Heritage
            {
                Environment = new Environment()
            };

            CurrentUser = UserJson.GetUsuarioJson();

            Icone = IconTheme.IconName("box");
            IconEnvironment = IconTheme.IconName("placeholder");
            IconCode = IconTheme.IconName("qrcode");
            IconDescription = IconTheme.IconName("document");
            IconDelete = IconTheme.IconName("cancel");
            IconName = IconTheme.IconName("name");
            IconState = IconTheme.IconName("verified");

            if (parameters.ContainsKey("heritage"))
                Heritage = (Heritage)parameters["heritage"];

            Environment = new Environment();

            if (Heritage.Environment != null)
                EnvironmentId = Heritage.Environment.Id;

            if (Heritage.State)
                Heritage.MessageState = "Patrimônio conferido";
            else
                Heritage.MessageState = "Patrimônio não encontrado";
            

            Heritage.SerializeEnvironmentId = false;

            Body = true;

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Environment> environments = await HeritageAPIService.GetAsyncEnvironments(CurrentUser.CompanyId);

                foreach (Environment environmente in environments)
                    Environments.Add(environmente);

                if (Environments.Count < 0)
                    Environments = new ObservableCollection<Environment>() { new Environment() { Name = "Sem ambientes para exibir" } };
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambiente" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                throw ex;
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
            var navigationParams = new NavigationParameters
            {
                {"heritage", heritage}
            };

            await NavigationService.NavigateAsync("AddMaintenance", navigationParams);
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
                        LoadMessage = "Atualizando patrimônio";

                        if (Environment.Id != EnvironmentId)
                        {
                            Heritage.EnvironmentId = Environment.Id;
                            Heritage.SerializeEnvironmentId = true;
                        }

                        bool up = await HeritageAPIService.PutAsync(Heritage);

                        if (up)
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio alterado com sucesso");
                        else
                        {
                            DialogParameters param = new DialogParameters
                            {
                                { "Message", "Erro ao atualizar patrimônio" },
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
                Body = true;
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao atualizar patrimônio" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                throw ex;
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

        private async void ExecuteDeleteCommand()
        {
            try
            {
                if (CurrentUser.UserLevel == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir o patrimônio?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Deletando patrimônio";

                        bool delete = await HeritageAPIService.DeleteAsync(Heritage);

                        if (delete)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio excluido com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir Patrimônio", "Ok");
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a exclusão");
            }
            catch (Exception)
            {
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir patrimônio", "Ok");
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

        #endregion

    }
}
