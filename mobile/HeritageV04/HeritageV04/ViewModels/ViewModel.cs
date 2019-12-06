using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageV04.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {

        #region Services

        protected INavigationService NavigationService { get; private set; }

        protected IHeritageAPIService HeritageAPIService { get; private set; }

        protected IPageDialogService PageDialogService { get; set; }

        protected IDialogService DialogService { get; set; }


        #endregion

        #region Variáveis

        public event EventHandler IsActiveChanged;

        string title = string.Empty;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        bool isNotBusy = true;

        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, RaiseIsActiveChanged);
        }

        private bool _null;

        public bool Null
        {
            get => _null;
            set => SetProperty(ref _null, value);
        }

        private bool _load;

        public bool Load
        {
            get => _load;
            set => SetProperty(ref _load, value);
        }

        private bool _body;

        public bool Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }

        string icone = string.Empty;

        public string Icone
        {
            get => icone;
            set => SetProperty(ref icone, value);
        }

        private IconTheme iconTheme;

        public IconTheme IconTheme
        {
            get => iconTheme;
            set => SetProperty(ref iconTheme, value);
        }

        private User currentUser;

        public User CurrentUser
        {
            get => currentUser;
            set => SetProperty(ref currentUser, value);
        }

        private UserJson userJson;

        public UserJson UserJson
        {
            get => userJson;
            set => SetProperty(ref userJson, value);
        }

        #endregion

        public ViewModelBase(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService)
        {
            NavigationService = navigationService;
            HeritageAPIService = heritageAPIService;
            PageDialogService = pageDialogService;
            DialogService = dialogService;

            IconTheme = new IconTheme();
            CurrentUser = new User();
            UserJson = new UserJson();
        }

        #region Métodos

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}