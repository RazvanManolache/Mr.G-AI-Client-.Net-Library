using System;
using System.Collections.Generic;
using System.Text;
using MrG.AI.Client.Data.Action;
using Newtonsoft.Json;

namespace MrG.AI.Client.Data.Socket
{
    public class SocketMessageData
    {
        public List<ActionApi> Actions { get; set; }

        public SocketMessageResult Result { get; set; }
        public List<SocketMessageOutput> Outputs { get; set; }
        [JsonProperty("run_uuid")]
        public string RunUuid { get; set; }

        public string RequestId { get; set; }
    }
}
