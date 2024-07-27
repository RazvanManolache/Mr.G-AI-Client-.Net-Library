using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Data.Socket
{
    public class SocketMessageResult
    {
        [JsonProperty("batch_request_uuids")]
        public List<string> RunUuids { get; set; }
    }
}
