using System.Collections.ObjectModel;
using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Prism.Commands;
using System.Threading.Tasks;
using HeritageV02MVVM.Events;
using System;
using HeritageV02MVVM.Utilities;

namespace HeritageV02MVVM.ViewModels
{
    public class PesquisarViewModel : ViewModelBase
    {

        #region Variáveis

        private ObservableCollection<Patrimonio> patrimonios;

        public ObservableCollection<Patrimonio> Patrimonios
        {
            get { return patrimonios; }
            set => SetProperty(ref patrimonios, value);
        }

        private ObservableCollection<Usuario> usuarios;

        public ObservableCollection<Usuario> Usuarios
        {
            get { return usuarios; }
            set => SetProperty(ref usuarios, value);
        }

        private ObservableCollection<Ambiente> ambientes;
        public ObservableCollection<Ambiente> Ambientes
        {
            get { return ambientes; }
            set { SetProperty(ref ambientes, value); }
        }

        IEventAggregator _eventAggregator;

        private string _pesquisa;

        public string Pesquisa
        {
            get { return _pesquisa; }
            set
            {
                if (SetProperty(ref _pesquisa, value))
                {
                    try
                    {
                        ObservableCollection<Patrimonio> patrimonios = new ObservableCollection<Patrimonio>();
                        foreach (Patrimonio patrimonio in Patrimonios)
                        {
                            string nome_patrimonio = patrimonio.Nome_patrimonio.ToLower();
                            string codigo_patrimonio = patrimonio.Codigo_patrimonio.ToLower();
                            string descricao = patrimonio.Descricao.ToLower();
                            string nome_ambiente = patrimonio.Nome_ambiente.ToLower();

                            if (nome_patrimonio.Contains(Pesquisa.ToLower()) || codigo_patrimonio.Contains(Pesquisa.ToLower()) || descricao.Contains(Pesquisa.ToLower()) || nome_ambiente.Contains(Pesquisa.ToLower()))
                                patrimonios.Add(patrimonio);
                        }
                        _eventAggregator.GetEvent<PesquisarPatrimoniosEvent>().Publish(patrimonios);
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao pesquisar patrimônio");
                    }

                    try
                    {
                        ObservableCollection<Ambiente> ambientes = new ObservableCollection<Ambiente>();
                        foreach (Ambiente ambiente in Ambientes)
                        {
                            string nome_ambiente = ambiente.Nome_ambiente.ToLower();
                            if (nome_ambiente.Contains(Pesquisa.ToLower()))
                                ambientes.Add(ambiente);
                        }
                        _eventAggregator.GetEvent<PesquisarAmbientesEvent>().Publish(ambientes);
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao pesquisar ambientes");
                    }

                    try
                    {
                        ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();
                        foreach (Usuario usuario in Usuarios)
                        {
                            string name = usuario.Name.ToLower();
                            string email = usuario.Email.ToLower();
                            if (name.Contains(Pesquisa.ToLower()) || email.Contains(Pesquisa.ToLower()))
                                usuarios.Add(usuario);
                        }
                        _eventAggregator.GetEvent<PesquisarUsuariosEvent>().Publish(usuarios);
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Erro ao pesquisar usuários");
                    }
                }
            }
        }

        #endregion

        public PesquisarViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Pesquisar";
            _eventAggregator = eventAggregator;
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Patrimonios = new ObservableCollection<Patrimonio>();
            Usuarios = new ObservableCollection<Usuario>();
            Ambientes = new ObservableCollection<Ambiente>();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Patrimonio> patrimonios = await HeritageAPIService.GetAsyncPatrimonios(UsuarioAtual.Id_empresa);

                Patrimonios.Clear();

                foreach (Patrimonio patrimonio in patrimonios)
                    Patrimonios.Add(patrimonio);

                IsBusy = true;
            }
            catch (Exception)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar patrimônios" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
            }
            finally
            {
                IsBusy = false;
            }

            try
            {
                ObservableCollection<Ambiente> ambientes = await HeritageAPIService.GetAsyncAmbientes(UsuarioAtual.Id_empresa);

                Ambientes.Clear();

                foreach (Ambiente ambiente in ambientes)
                    Ambientes.Add(ambiente);

                IsBusy = true;
            }
            catch (Exception)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambientes" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
            }
            finally
            {
                IsBusy = false;
            }

            try
            {
                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios(UsuarioAtual.Id_empresa);

                Usuarios.Clear();

                foreach (Usuario usuario in usuarios)
                {
                    if (UsuarioAtual.Id != usuario.Id)
                        Usuarios.Add(usuario);
                }

                IsBusy = true;
            }
            catch (Exception)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
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
