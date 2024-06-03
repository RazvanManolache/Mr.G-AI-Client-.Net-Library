using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Data.Socket
{
    public class SocketMessageResult
    {
        [JsonProperty("run_uuid")]
        public List<string> RunUuids { get; set; }
    }
}
