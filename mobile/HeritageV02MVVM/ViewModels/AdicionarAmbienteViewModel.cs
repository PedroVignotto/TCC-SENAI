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
    public class AdicionarAmbienteViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand AdicionarAmbienteCommand { get; set; }

        #endregion

        #region Variáveis 

        private Ambiente ambiente;

        public Ambiente Ambiente
        {
            get => ambiente;
            set => SetProperty(ref ambiente, value);
        }

        private Usuario usuario;

        public Usuario Usuario
        {
            get => usuario;
            set => SetProperty(ref usuario, value);
        }

        private ObservableCollection<Usuario> usuarios;

        public ObservableCollection<Usuario> Usuarios
        {
            get => usuarios;
            set => SetProperty(ref usuarios, value);
        }


        #endregion

        public AdicionarAmbienteViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Adicionar ambiente";
            Icone = "";

            AdicionarAmbienteCommand = new DelegateCommand(ExecuteAdicionarAmbienteCommand);
        }

        #region Métodos 

        public override async void Initialize(INavigationParameters parameters)
        {
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();
            Usuario = new Usuario();
            Usuarios = new ObservableCollection<Usuario>();
            Ambiente = new Ambiente();

            Body = true;

            UsuarioAtual = JsonUsuario.GetUsuarioJson();
            Icone = Icon.IconName("placeholder");

            await LoadAsync();
        }

        private async Task LoadAsync()
        {

            try
            {
                Usuarios.Clear();

                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios(UsuarioAtual.Id_empresa);

                foreach (Usuario usuario in usuarios)
                {
                    if (usuario.Id_nivel_usuario == 2)
                        Usuarios.Add(usuario);

                }

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
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

        private async void ExecuteAdicionarAmbienteCommand()
        {
            try
            {
                IsBusy = true;

                Load = true;
                Body = false;

                Ambiente.Id_empresa = UsuarioAtual.Id_empresa;

                if (Validation(Ambiente))
                {
                    string validation = await HeritageAPIService.ValidationAsyncAmbiente(Ambiente, UsuarioAtual.Id_empresa);

                    if (validation == null)
                    {
                        Ambiente.Id_usuario = Usuario.Id;
                        var insert = await HeritageAPIService.SetAsync(Ambiente);

                        if (insert == false)
                        {
                            DialogParameters param = new DialogParameters
                            {
                                { "Message", "Erro ao adicionar ambiente" },
                                { "Title", "Erro" },
                                { "Icon", Icon.IconName("bug") }
                            };

                            DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                        }
                        else
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Ambiente adicionado com sucesso");
                            Ambiente = null;
                        }
                    }
                    else
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert(validation + " já cadastrado");
                    }
                }
                else
                {
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Preencha todos os campos");
                }

            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao adicionar ambiente" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
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

        private bool Validation(Ambiente ambiente)
        {
            if (ambiente.Nome_ambiente == null || Usuario.Id == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void Destroy()
        {
            UsuarioAtual = null;
            JsonUsuario = null;
            Icon = null;
            Usuario = null;
            Usuarios = null;
            Ambiente = null;
        }

        #endregion
    }
}
