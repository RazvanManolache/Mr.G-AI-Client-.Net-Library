using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Data.Action
{
    public class ActionParameterCollectionValue : ObservableObject
    {
        private string _value;
        [JsonProperty("v")]
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        private int _count;
        [JsonProperty("#")]
        public int Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }
    }
}
