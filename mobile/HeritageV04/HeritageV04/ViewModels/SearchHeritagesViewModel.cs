using HeritageV04.Events;
using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV04.ViewModels
{
    class SearchHeritagesViewModel : ViewModelBase
    {

        #region Commands

        private DelegateCommand<Heritage> _DisplayHeritageCommand;
        public DelegateCommand<Heritage> DisplayHeritageCommand => _DisplayHeritageCommand ?? (_DisplayHeritageCommand = new DelegateCommand<Heritage>(async (itemSelect) => await ExecuteDisplayHeritageCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variables

        private ObservableCollection<Heritage> heritages;

        public ObservableCollection<Heritage> Heritages
        {
            get => heritages;
            set => SetProperty(ref heritages, value);
        }

        readonly IEventAggregator _eventAggregator;

        #endregion

        public SearchHeritagesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Patrimônios";
            Icone = "boxIcon.png";

            Heritages = new ObservableCollection<Heritage>();
            _eventAggregator = eventAggregator;

            Body = true;

            _eventAggregator.GetEvent<EventCommunicationHeritages>().Subscribe(SearchHeritage);
        }

        #region Methods
        public void SearchHeritage(ObservableCollection<Heritage> heritages)
        {
            Heritages.Clear();

            foreach (Heritage heritage in heritages)
                Heritages.Add(heritage);
        }

        private async Task ExecuteDisplayHeritageCommand(Heritage heritage)
        {
            var navigationParams = new NavigationParameters
            {
                {"heritage", heritage}
            };

            await NavigationService.NavigateAsync("DisplayHeritage", navigationParams);
        }
        #endregion

    }
}
