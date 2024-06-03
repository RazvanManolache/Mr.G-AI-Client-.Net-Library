using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MrG.AI.Client.Data.Action
{
    public class ActionParameterCollection : ObservableObject
    {
        private ActionParameterCollectionValue[] _values;


        [JsonProperty("c")]
        public ActionParameterCollectionValue[] Values
        {
            get { return _values; }
            set { SetProperty(ref _values, value); }
        }
        private float _count;

        [JsonProperty("#")]
        public float Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }

        private ActionParameterCollectionInterval _interval;

        [JsonProperty("i")]
        public ActionParameterCollectionInterval Interval
        {
            get { return _interval; }
            set { SetProperty(ref _interval, value); }
        }
        private string _customValue;
        [JsonProperty("?")]
        public string CustomValue
        {
            get { return _customValue; }
            set { SetProperty(ref _customValue, value); }
        }

    }
}
