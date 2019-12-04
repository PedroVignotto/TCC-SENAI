using HeritageV02MVVM.Models;
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
    public class PatrimoniosSelectViewModel : ViewModelBase, IConfirmNavigation
    {


        #region Commands

        private DelegateCommand<Patrimonio> _ExibirPatrimonioCommand;
        public DelegateCommand<Patrimonio> ExibirPatrimonioCommand => _ExibirPatrimonioCommand ?? (_ExibirPatrimonioCommand = new DelegateCommand<Patrimonio>(async (itemSelect) => await ExecuteExibirPatrimonioCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        private ObservableCollection<Patrimonio> patrimonios;
        public ObservableCollection<Patrimonio> Patrimonios
        {
            get { return patrimonios; }
            set { SetProperty(ref patrimonios, value); }
        }

        private string _codigo;

        public string Codigo
        {
            get { return _codigo; }
            set => SetProperty(ref _codigo, value);
        }

        private string _loadMessage;
        public string LoadMessage
        {
            get { return _loadMessage; }
            set { SetProperty(ref _loadMessage, value); }
        }

        #endregion

        public PatrimoniosSelectViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Verificar patrimônios";
        }

        #region Métodos

        public override void Initialize(INavigationParameters parameters)
        {
            Patrimonios = new ObservableCollection<Patrimonio>();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            if (parameters.ContainsKey("patrimonios"))
            {
                ObservableCollection<Patrimonio> patrimonios = (ObservableCollection<Patrimonio>)parameters["patrimonios"];

                foreach (Patrimonio patrimonio in patrimonios)
                {
                    patrimonio.Estado_patrimonio = false;
                    Patrimonios.Add(patrimonio);
                }
                    
            }

        }

        private async Task ExecuteExibirPatrimonioCommand(Patrimonio patrimonio)
        {
            var navigationParams = new NavigationParameters
            {
                {"patrimonio", patrimonio}
            };

            await NavigationService.NavigateAsync("ExibirPatrimonio", navigationParams);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.Back)
            {
                if (parameters.ContainsKey("codigo"))
                    Codigo = (string)parameters["codigo"];

                bool exist = true;

                foreach (Patrimonio patrimonio in Patrimonios)
                {
                    if (Codigo == patrimonio.Codigo_patrimonio)
                    {
                        patrimonio.Estado_patrimonio = true;
                        if (await Verficar(patrimonio))
                        {
                            Patrimonios.Remove(patrimonio);
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

        private async Task<bool> Verficar(Patrimonio patrimonio)
        {
            bool up = false;

            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Fazer a verificação do patrimônio?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Atualizando patrimônio";

                        up = await HeritageAPIService.PutAsync(patrimonio);

                        if (up)
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio verificado com sucesso");
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao verificar patrimônio", "Ok");

                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a alteração");
            }
            catch (Exception ex)
            {
                Body = true;
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao verificar patrimônio", "Ok");

                throw ex;
            }
            finally
            {
                Load = false;
                Body = true;
            }

            return up;
        }

        public bool CanNavigate(INavigationParameters parameters)
        {
            return true;
        }

        #endregion

    }
}
