using Prism.Mvvm;

namespace HeritageV04.Models
{
    public class Itens : BindableBase
    {

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value); }
        }

        private string page;
        public string Page
        {
            get { return page; }
            set { SetProperty(ref page, value); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

    }
}
