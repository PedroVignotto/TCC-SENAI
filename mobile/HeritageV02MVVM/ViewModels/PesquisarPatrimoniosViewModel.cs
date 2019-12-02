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
    public class PesquisarPatrimoniosViewModel : ViewModelBase
    {

        #region Commands

        private DelegateCommand<Patrimonio> _ExibirPatrimonioCommand;
        public DelegateCommand<Patrimonio> ExibirPatrimonioCommand => _ExibirPatrimonioCommand ?? (_ExibirPatrimonioCommand = new DelegateCommand<Patrimonio>(async (itemSelect) => await ExecuteExibirPatrimonioCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        IEventAggregator _eventAggregator;

        private ObservableCollection<Patrimonio> patrimonios;

        public ObservableCollection<Patrimonio> Patrimonios
        {
            get { return patrimonios; }
            set => SetProperty(ref patrimonios, value);
        }

        #endregion

        public PesquisarPatrimoniosViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IHeritageAPIService heritageAPIService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Patrimônios";
            Icone = "boxIcon.png";

            _eventAggregator = eventAggregator;

            Body = true;

            Patrimonios = new ObservableCollection<Patrimonio>();

            _eventAggregator.GetEvent<PesquisarPatrimoniosEvent>().Subscribe(PesquisarPatrimonio);
        }

        #region Métodos

        public void PesquisarPatrimonio(ObservableCollection<Patrimonio> patrimonios)
        {
            Patrimonios.Clear();

            foreach (Patrimonio patrimonio in patrimonios)
                Patrimonios.Add(patrimonio);
        }

        private async Task ExecuteExibirPatrimonioCommand(Patrimonio patrimonio)
        {
            var navigationParams = new NavigationParameters
            {
                {"patrimonio", patrimonio}
            };

            await NavigationService.NavigateAsync("ExibirPatrimonio", navigationParams);
        } 

        #endregion

    }
}
