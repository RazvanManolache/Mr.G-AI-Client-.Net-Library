using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Enum
{
    public enum RequestStatus
    {
        Requested,
        Queued,
        Processing,
        Finished,
        Failed,
        Unknown
    }
}
