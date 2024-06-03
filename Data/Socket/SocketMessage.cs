using MrG.AI.Client.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Data.Socket
{
    public class SocketMessage
    {
        [JsonProperty("type")]
        public string InternalType { get; set; }
        public SocketMessageEnum? Type
        {
            get
            {
                if (InternalType != null)
                {
                    if (System.Enum.TryParse<SocketMessageEnum>(InternalType, true, out var type))
                    {
                        return type;
                    }
                }
                return SocketMessageEnum.NotMapped;
            }
        }
        [JsonProperty("data")]
        public SocketMessageData Data { get; internal set; }
    }
}
