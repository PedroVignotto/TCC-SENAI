using HeritageV04.Services.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Forms;
using ZXing;

namespace HeritageV04.ViewModels
{
    public class ScanViewModel : ViewModelBase
    {

        private string _codigo;

        public string Codigo
        {
            get { return _codigo; }
            set => SetProperty(ref _codigo, value);
        }

        private string _envio;

        public string Envio
        {
            get { return _envio; }
            set => SetProperty(ref _envio, value);
        }


        private bool _isAnalyzing;

        public bool IsAnalyzing
        {
            get { return _isAnalyzing; }
            set => SetProperty(ref _isAnalyzing, value);
        }

        private bool _isScanning;

        public bool IsScanning
        {
            get { return _isScanning; }
            set => SetProperty(ref _isScanning, value);
        }

        private Result _result;

        public Result Result
        {
            get { return _result; }
            set
            {
                if (SetProperty(ref _result, value))
                {
                    IsScanning = false;
                    IsAnalyzing = false;
                }
            }
        }

        public Command ScanCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsAnalyzing = false;
                    IsScanning = false;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var navigationParams = new NavigationParameters
                        {
                            {"code", Result.Text}
                        };

                        if (Envio == "Main")
                            await NavigationService.NavigateAsync("Acoes", navigationParams);
                        else
                            await NavigationService.GoBackAsync(navigationParams);
                    });

                    IsAnalyzing = true;
                    IsScanning = true;
                });
            }
        }

        public DelegateCommand CancelarCommand { get; private set; }

        protected ScanViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            CancelarCommand = new DelegateCommand(ExecuteCancelarCommand);

            IsScanning = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("scanner"))
                Envio = (string)parameters["scanner"];
        }

        private async void ExecuteCancelarCommand()
        {
            await NavigationService.GoBackAsync();
        }

    }
}
