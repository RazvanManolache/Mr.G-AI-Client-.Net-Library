using MrG.AI.Client.Data;
using MrG.AI.Client.Database;
using MrG.AI.Client.Enum;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.Database.Data
{
    public class Request : BaseTable
    {
        public string ActionUuid { get; set; }
        public string ActionName { get; set; }

      
        public string Value { get; set; }
        public bool Seen { get; internal set; }

        public RequestStatus RequestStatus { get; set; }

       

        public Color StatusColor
        {
            get
            {
                switch (RequestStatus)
                {
                    case RequestStatus.Requested:
                        return Color.Blue;
                    case RequestStatus.Processing:
                        return Color.Orange;
                    case RequestStatus.Failed:
                        return Color.Red;
                    case RequestStatus.Finished:
                        return Color.Green;
                    case RequestStatus.Queued:
                            return Color.Yellow;

                    
                    default:
                        return Color.Black;
                }
            }
        }

        private List<OutputItem>  _OutputItems = new List<OutputItem>();
        [Ignore]
        public List<OutputItem> OutputItems
        {
            get
            {
                if(_OutputItems.Count == 0)
                    UpdateOutputItems();
                return _OutputItems;

            }
            set
            {
                SetProperty(ref _OutputItems, value);
            }
        }

        public Request() : base()
        {
            RequestStatus = RequestStatus.Requested;
        }

        public async Task<List<RunItem>> GetRunItems()
        {
                if (Server.Instance != null && DatabaseHelper.Instance!=null)
                {
                    return (await DatabaseHelper.Instance.GetAll<RunItem>(q => q.RequestUuid == Uuid)).ToList();
                }
                return new List<RunItem>();
            
        }

        public async void UpdateOutputItems()
        {
            OutputItems = await GetOutputItems();
        }

        public async Task<List<OutputItem>> GetOutputItems()
        {
            var runItems = await GetRunItems();
            var outputItems = new List<OutputItem>();
            foreach (var runItem in runItems)
            {
                outputItems.AddRange(await runItem.GetOutputItems());
            }
            return outputItems;
        }

       

        public Request(JObject request) : this()
        {

            if (request.ContainsKey("api"))
            {
                var val = request.GetValue("api");
                if (val != null)
                    ActionUuid = val.ToString();
                if (ActionUuid != null && Server.Instance != null)
                {
                    var action = Server.Instance.Actions.Where(a => a.Uuid == ActionUuid).FirstOrDefault();
                    if (action != null)
                    {
                        ActionName = action.Name;
                    }
                }
            }

            if (request.ContainsKey("params"))
            {
                var val = request.GetValue("params");
                if (val != null)
                    Value = val.ToString();
            }
            if (request.ContainsKey("request_status"))
            {
                var val = request.GetValue("request_status");
                if (val != null)
                    RequestStatus = (RequestStatus)System.Enum.Parse(typeof(RequestStatus), val.ToString());
            }
        }
    }
}
