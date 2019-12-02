using HeritageV02MVVM.Events;
using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.ViewModels
{
    public class PesquisarAmbientesViewModel : ViewModelBase
    {

        #region Commands

        private DelegateCommand<Ambiente> _ExibirAmbienteCommand;
        public DelegateCommand<Ambiente> ExibirAmbienteCommand => _ExibirAmbienteCommand ?? (_ExibirAmbienteCommand = new DelegateCommand<Ambiente>(async (itemSelect) => await ExecuteExibirAmbienteCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        private ObservableCollection<Ambiente> ambientes;
        public ObservableCollection<Ambiente> Ambientes
        {
            get { return ambientes; }
            set { SetProperty(ref ambientes, value); }
        }

        IEventAggregator _eventAggregator;

        #endregion

        public PesquisarAmbientesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Ambientes";
            Icone = "placeholderIcon.png";

            Ambientes = new ObservableCollection<Ambiente>();
            _eventAggregator = eventAggregator;

            Body = true;

            _eventAggregator.GetEvent<PesquisarAmbientesEvent>().Subscribe(PesquisarAmbiente);
        }

        #region Métodos

        public void PesquisarAmbiente(ObservableCollection<Ambiente> ambientes)
        {
            Ambientes.Clear();

            foreach (Ambiente ambiente in ambientes)
            {
                Ambientes.Add(ambiente);
            }
        }

        private async Task ExecuteExibirAmbienteCommand(Ambiente ambiente)
        {
            var navigationParams = new NavigationParameters
            {
                {"ambiente", ambiente}
            };

            await NavigationService.NavigateAsync("ExibirAmbiente", navigationParams);
        }

        #endregion

    }
}
