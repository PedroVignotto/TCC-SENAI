using HeritageV04.Models;
using Prism.Events;
using System.Collections.ObjectModel;

namespace HeritageV04.Events
{
    public class EventCommunicationUsers : PubSubEvent<ObservableCollection<User>>
    {



    }
}
