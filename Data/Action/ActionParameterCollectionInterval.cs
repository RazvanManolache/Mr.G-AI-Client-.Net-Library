using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Data.Action
{
    public class ActionParameterCollectionInterval : ObservableObject
    {
        private float _min;

        [JsonProperty("n")]
        public float Min
        {
            get { return _min; }
            set { SetProperty(ref _min, value); }
        }
        private float _max;
        [JsonProperty("x")]
        public float Max
        {
            get { return _max; }
            set { SetProperty(ref _max, value); }
        }
        private float _step;
        [JsonProperty("s")]
        public float Step
        {
            get { return _step; }
            set { SetProperty(ref _step, value); }
        }
    }
}
