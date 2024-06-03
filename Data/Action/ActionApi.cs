using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.Data.Action
{
    public class ActionApi : ObservableObject
    {
        private string _uuid;
        [JsonProperty("uuid")]
        public string Uuid
        {
            get { return _uuid; }
            set { SetProperty(ref _uuid, value); }
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _endpoint;
        [JsonProperty("endpoint")]
        public string Endpoint
        {
            get { return _endpoint; }
            set { SetProperty(ref _endpoint, value); }
        }

        private int _runs;
        [JsonProperty("runs")]
        public int Runs
        {
            get { return _runs; }
            set { SetProperty(ref _runs, value); }
        }

        private string _description;
        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private List<ActionParameter> _parameters;
        [JsonProperty("parameters")]
        public List<ActionParameter> Parameters
        {
            get { return _parameters; }
            set { SetProperty(ref _parameters, value); }
        }



    }
}
