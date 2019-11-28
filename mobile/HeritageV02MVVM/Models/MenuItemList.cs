using Prism.Mvvm;

namespace HeritageV02MVVM.Models
{
    public class MenuItemList : BindableBase
    {

        private string _nome;

        public string Nome
        {
            get => _nome;
            set => SetProperty(ref _nome, value);
        }

        private string _icon;

        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private string _pagina;

        public string Pagina
        {
            get => _pagina;
            set => SetProperty(ref _pagina, value);
        }

        private string _descricao;

        public string Descricao
        {
            get => _descricao;
            set => SetProperty(ref _descricao, value);
        }

    }
}
