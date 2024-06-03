using MrG.AI.Client.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.EventArgs
{
    public class LocalServerEventArgs : System.EventArgs
    {
        public ServerType ServerType { get {  return ServerType.Local; } }
        public string ServerName { get; set; }
        public string ServerAddress { get; set; }
       

    }
}
