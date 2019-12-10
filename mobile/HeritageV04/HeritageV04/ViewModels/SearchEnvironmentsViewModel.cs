using HeritageV04.Events;
using HeritageV04.Services.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Environment = HeritageV04.Models.Environment;

namespace HeritageV04.ViewModels
{
    public class SearchEnvironmentsViewModel : ViewModelBase
    {

        #region Commands
        private DelegateCommand<Environment> _DisplayEnvironmentCommand;
        public DelegateCommand<Environment> DisplayEnvironmentCommand => _DisplayEnvironmentCommand ?? (_DisplayEnvironmentCommand = new DelegateCommand<Environment>(async (itemSelect) => await ExecuteDisplayEnvironmentCommand(itemSelect), (itemSelect) => !IsBusy));
        #endregion

        #region Variables
        private ObservableCollection<Environment> environments;
        public ObservableCollection<Environment> Environments
        {
            get { return environments; }
            set { SetProperty(ref environments, value); }
        }

        readonly IEventAggregator _eventAggregator; 
        #endregion

        public SearchEnvironmentsViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Ambientes";
            Icone = "placeholderIcon.png";

            Environments = new ObservableCollection<Environment>();
            _eventAggregator = eventAggregator;

            Body = true;

            _eventAggregator.GetEvent<EventCommunicationEnvironments>().Subscribe(SearchEnvironment);
        }

        #region Methods
        public void SearchEnvironment(ObservableCollection<Environment> environments)
        {
            Environments.Clear();

            foreach (Environment environment in environments)
                Environments.Add(environment);
        }

        private async Task ExecuteDisplayEnvironmentCommand(Environment environment)
        {
            var navigationParams = new NavigationParameters
            {
                {"environment", environment}
            };

            await NavigationService.NavigateAsync("DisplayEnvironment", navigationParams);
        } 
        #endregion

    }

}
