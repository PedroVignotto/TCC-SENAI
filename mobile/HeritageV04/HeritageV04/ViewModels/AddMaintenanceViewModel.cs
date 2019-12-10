using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;

namespace HeritageV04.ViewModels
{
    public class AddMaintenanceViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand AddCommand => new DelegateCommand(ExecuteAddCommand);

        #endregion

        #region Variables
        private Maintenance maintenance;
        public Maintenance Maintenance
        {
            get { return maintenance; }
            set { SetProperty(ref maintenance, value); }
        }

        private Heritage heritage;
        public Heritage Heritage
        {
            get { return heritage; }
            set { SetProperty(ref heritage, value); }
        }

        #endregion

        public AddMaintenanceViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Manutenção";
        }

        public override void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            Maintenance = new Maintenance();
            Heritage = new Heritage();

            Body = true;

            CurrentUser = UserJson.GetUsuarioJson();
            Icone = IconTheme.IconName("tools");

            if (parameters.ContainsKey("heritage"))
                Heritage = (Heritage)parameters["heritage"];

            Maintenance.CompanyId = Heritage.CompanyId;
            Maintenance.Code = Heritage.Code;
            Maintenance.HeritageId = Heritage.Id;
        }

        private async void ExecuteAddCommand()
        {
            try
            {
                Body = false;
                Load = true;

                if (Maintenance.Problem != null)
                {
                    Repost repost = await HeritageAPIService.PostAsync(Maintenance);

                    if (repost.Success)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Manutenção adicionada com sucesso");
                        await NavigationService.GoBackAsync();
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
                Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao adicionar manutenção");
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

        private void CloseDialogCallback(IDialogResult obj)
        {
            throw new NotImplementedException();
        }
    }
}
