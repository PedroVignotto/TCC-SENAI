﻿using HeritageV02MVVM.Models;
using Prism.Events;
using System.Collections.ObjectModel;

namespace HeritageV02MVVM.Events
{
    public class PesquisarAmbientesEvent : PubSubEvent<ObservableCollection<Ambiente>>
    {

    }
}
