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
    public class SearchUsersViewModel : ViewModelBase
    {

        #region Commands
        private DelegateCommand<User> _DisplayUserCommand;
        public DelegateCommand<User> DisplayUserCommand => _DisplayUserCommand ?? (_DisplayUserCommand = new DelegateCommand<User>(async (itemSelect) => await ExecuteDisplayUserCommand(itemSelect), (itemSelect) => !IsBusy));
        #endregion

        #region Variables
        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        readonly IEventAggregator _eventAggregator; 
        #endregion

        public SearchUsersViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Usuários";
            Icone = "avatarIcon.png";

            Users = new ObservableCollection<User>();
            _eventAggregator = eventAggregator;

            Body = true;

            _eventAggregator.GetEvent<EventCommunicationUsers>().Subscribe(SearchUser);
        }

        #region Methods
        public void SearchUser(ObservableCollection<User> users)
        {
            Users.Clear();

            foreach (User user in users)
                Users.Add(user);
        }

        private async Task ExecuteDisplayUserCommand(User user)
        {
            var navigationParams = new NavigationParameters
            {
                {"user", user}
            };

            if (user.UserLevel == 1)
                await NavigationService.NavigateAsync("DisplayAdmin", navigationParams);
            else if (user.UserLevel == 2)
                await NavigationService.NavigateAsync("DisplayManager", navigationParams);
            else if (user.UserLevel == 3)
                await NavigationService.NavigateAsync("DisplaySupport", navigationParams);
        }
        #endregion

    }
}
