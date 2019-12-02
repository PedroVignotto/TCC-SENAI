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
    public class AdicionarPatrimonioViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand AdicionarPatrimonioCommand { get; private set; }

        #endregion

        #region Variáveis

        private ObservableCollection<Ambiente> ambientes;

        public ObservableCollection<Ambiente> Ambientes
        {
            get => ambientes;
            set => SetProperty(ref ambientes, value);
        }

        private Patrimonio patrimonio;

        public Patrimonio Patrimonio
        {
            get => patrimonio;
            set => SetProperty(ref patrimonio, value);
        }

        private Ambiente ambiente;

        public Ambiente Ambiente
        {
            get => ambiente;
            set => SetProperty(ref ambiente, value);
        }

        #endregion

        public AdicionarPatrimonioViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Adicionar patrimônio";

            AdicionarPatrimonioCommand = new DelegateCommand(ExecuteAdicionaPatrimonioCommand);

        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Ambiente = new Ambiente();
            Ambientes = new ObservableCollection<Ambiente>();
            UsuarioAtual = new Usuario();
            Icon = new Icon();
            JsonUsuario = new JsonUsuario();
            Patrimonio = new Patrimonio();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();
            Icone = Icon.IconName("box");

            Body = true;

            await LoadAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.Back)
            {
                if (parameters.ContainsKey("codigo"))
                    Patrimonio.Codigo_patrimonio = (string)parameters["codigo"];
            }
        }

        private async Task LoadAsync()
        {
            try
            {
                Ambientes.Clear();

                ObservableCollection<Ambiente> ambientes = await HeritageAPIService.GetAsyncAmbientes(UsuarioAtual.Id_empresa);

                foreach (Ambiente ambiente in ambientes)
                {
                    Ambientes.Add(ambiente);
                }

                IsBusy = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambientes" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarPatrimonio");
            }
            finally
            {
                IsBusy = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        private bool Validation(Patrimonio patrimonio)
        {
            if (patrimonio.Nome_patrimonio == null || patrimonio.Codigo_patrimonio == null || patrimonio.Descricao == null || Ambiente.Id == null)
                return false;
            else
                return true;
        }

        private async void ExecuteAdicionaPatrimonioCommand()
        {
            try
            {
                Body = false;
                Load = true;

                if (Validation(Patrimonio))
                {
                    Patrimonio.Id_ambiente = Ambiente.Id;
                    Patrimonio.Id_empresa = UsuarioAtual.Id_empresa;

                    string validation = await HeritageAPIService.ValidationAsyncPatrimonio(Patrimonio, UsuarioAtual.Id_empresa);

                    if (validation == null)
                    {
                        var insert = await HeritageAPIService.SetAsync(Patrimonio);

                        if (insert == false)
                        {
                            var param = new DialogParameters
                            {
                                { "Message", "Erro ao adicionar patrimônio" },
                                { "Title", "Erro" },
                                { "Icon", Icon.IconName("bug") }
                            };

                            DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                        }
                        else
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio adicionado com sucesso");
                            Patrimonio = null;
                        }
                    }
                    else
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert(validation + " já cadastrado");

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
