using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace HeritageV02MVVM.ViewModels
{
    public class ExibirSuporteViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand SalvarCommand => new DelegateCommand(ExecuteSalvarCommand);

        public DelegateCommand ExcluirCommand => new DelegateCommand(ExecuteExcluirCommand);

        #endregion

        #region Variáveis

        private Usuario _usuario;
        public Usuario Usuario
        {
            get => _usuario;
            set => SetProperty(ref _usuario, value);
        }

        private string _nivel;
        public string Nivel
        {
            get { return _nivel; }
            set => SetProperty(ref _nivel, value);
        }

        private ObservableCollection<string> niveis;
        public ObservableCollection<string> Niveis
        {
            get { return niveis; }
            set { SetProperty(ref niveis, value); }
        }

        private string _iconEmail;

        public string IconEmail
        {
            get => _iconEmail;
            set => SetProperty(ref _iconEmail, value);
        }

        private string _iconNivel;

        public string IconNivel
        {
            get => _iconNivel;
            set => SetProperty(ref _iconNivel, value);
        }

        private string _iconNome;

        public string IconNome
        {
            get => _iconNome;
            set => SetProperty(ref _iconNome, value);
        }

        private string _iconExcluir;

        public string IconExcluir
        {
            get => _iconExcluir;
            set => SetProperty(ref _iconExcluir, value);
        }

        private string _loadMessage;

        public string LoadMessage
        {
            get { return _loadMessage; }
            set => SetProperty(ref _loadMessage, value);
        } 

        #endregion

        public ExibirSuporteViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Exibir suporte";
        }

        public override void Initialize(INavigationParameters parameters)
        {
            UsuarioAtual = new Usuario();
            Icon = new Icon();
            JsonUsuario = new JsonUsuario();

            Nivel = "Suporte";

            Niveis = new ObservableCollection<string>()
            {
                "Administrador geral", "Gerenciador de ambiente", "Suporte"
            };

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            IconEmail = Icon.IconName("email");
            IconNivel = Icon.IconName("controls");
            IconExcluir = Icon.IconName("cancel");
            IconNome = Icon.IconName("name");
            Icone = Icon.IconName("avatar");

            Body = true;
            Load = false;

            if (parameters.ContainsKey("usuario"))
                Usuario = (Usuario)parameters["usuario"];
        }

        private async void ExecuteSalvarCommand()
        {
            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja salvar as alterações?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Atualizando usuário";

                        if (Nivel == "Administrador geral")
                            Usuario.Id_nivel_usuario = 1;
                        else if (Nivel == "Gerenciador de ambiente")
                            Usuario.Id_nivel_usuario = 2;
                        else if (Nivel == "Suporte")
                            Usuario.Id_nivel_usuario = 3;

                        bool up = await HeritageAPIService.PutAsync(Usuario);

                        if (up)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Usuário atualizado com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao deletar usuário", "Ok");
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a alteração");
            }
            catch (Exception)
            {
                Load = false;
                Body = true;
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao alterar usuário", "Ok");
            }
        }

        private async void ExecuteExcluirCommand()
        {
            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir o usuario?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Atualizando usuário";

                        bool delete = await HeritageAPIService.DeleteAsync(Usuario);

                        if (delete)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Usuário excluido com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir usuário", "Ok");
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a exclusão");
            }
            catch (Exception)
            {
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir usuário", "Ok");
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

    }
}
