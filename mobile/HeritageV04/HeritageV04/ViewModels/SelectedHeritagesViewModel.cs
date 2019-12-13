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
    public class SelectedHeritagesViewModel : ViewModelBase
    {

        #region Commands
        private DelegateCommand<Heritage> _DisplayHeritageCommand;
        public DelegateCommand<Heritage> DisplayHeritageCommand => _DisplayHeritageCommand ?? (_DisplayHeritageCommand = new DelegateCommand<Heritage>(async (itemSelect) => await ExecuteDisplayHeritageCommand(itemSelect), (itemSelect) => !IsBusy));
        #endregion

        #region Variables
        private ObservableCollection<Heritage> heritages;
        public ObservableCollection<Heritage> Heritages
        {
            get { return heritages; }
            set { SetProperty(ref heritages, value); }
        }

        private string code;
        public string Code
        {
            get { return code; }
            set { SetProperty(ref code, value); }
        }

        private string loadMessage;
        public string LoadMessage
        {
            get { return loadMessage; }
            set { SetProperty(ref loadMessage, value); }
        }
        #endregion

        public SelectedHeritagesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Verificar patrimônios";
        }

        private async Task ExecuteDisplayHeritageCommand(Heritage heritage)
        {
            var navigationParams = new NavigationParameters
            {
                {"heritage", heritage}
            };

            await NavigationService.NavigateAsync("DisplayHeritage", navigationParams);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            Heritages = new ObservableCollection<Heritage>();
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();

            CurrentUser = UserJson.GetUsuarioJson();

            if (parameters.ContainsKey("heritages"))
            {
                ObservableCollection<Heritage> heritages = (ObservableCollection<Heritage>)parameters["heritages"];

                foreach (Heritage heritage in heritages)
                    Heritages.Add(heritage);

            }

        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.Back)
            {
                if (parameters.ContainsKey("code"))
                    Code = (string)parameters["code"];

                bool exist = true;

                foreach (Heritage heritage in Heritages)
                {
                    if (Code == heritage.Code)
                    {
                        heritage.State = true;

                        if (await Verification(heritage))
                        {
                            Heritages.Remove(heritage);
                            exist = true;
                            break;
                        }
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao verificar patrimônio", "Ok");

                    }
                    else
                        exist = false;
                }

                if (exist == false)
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio não existe neste ambiente");
            }
        }

        private async Task<bool> Verification(Heritage heritage)
        {
            try
            {
                Load = true;
                Body = false;
                LoadMessage = "Verificando patrimônio";

                heritage.SerializeCode = false;
                heritage.State = true;

                bool up = await HeritageAPIService.PutAsync(heritage);

                if (up)
                {
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio verificado com sucesso");

                    Load = false;
                    Body = true;

                    return true;
                }
                else
                {
                    DialogParameters param = new DialogParameters
                    {
                        { "Message", "Erro ao atualizar patrimônio" },
                        { "Title", "Erro" },
                        { "Icon", IconTheme.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                    Load = false;
                    Body = true;

                    return false;
                }
            }
            catch (Exception)
            {
                Body = true;
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao atualizar patrimônio" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                Load = false;
                Body = true;

                return false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

    }
}
