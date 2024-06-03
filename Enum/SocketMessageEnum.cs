using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Enum
{
    public enum SocketMessageEnum
    {
        ActionDefinitions,
        ApiUpdated,
        ExecutionFinished,
        GetStatusResponse,
        ExecuteApiQueued,
        ResultPushed,
        NotMapped
    }
}
