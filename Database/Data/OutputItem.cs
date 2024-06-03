using MrG.AI.Client.Data.Socket;
using MrG.AI.Client.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Database.Data
{
    public class OutputItem : BaseTable
    {

        [Indexed]
        public string QueuedItemUuid { get; set; }

        public ResultType Type { get; set; }

        public string Result { get; set; }


        public OutputItem() : this(string.Empty, ResultType.None, string.Empty)
        {
        }

        public OutputItem(SocketMessageOutput result, string runUuid)
        {
            Uuid = Guid.NewGuid().ToString();
            QueuedItemUuid = runUuid;
            if(System.Enum.TryParse(result.Type,true, out ResultType type))
            {
                Type = type;
            }
            else
            {
                Type = ResultType.None;
            }


            if (result.Contents == null)
            {
                Result = string.Empty;
            }
            else
            {
                if (Type == ResultType.File || Type == ResultType.Images || Type == ResultType.Video)
                {
                    //decode contents from base64, write to file , and store path in Result
                    string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Uuid+ result.Name);
                    System.IO.File.WriteAllBytes(path, Convert.FromBase64String(result.Contents));
                    Result = path;
                }
                else
                {
                    Result = result.Contents;
                }
            }
                

        }

        public OutputItem(string queuedItemUuid, ResultType type, string result) : base()
        {
            Uuid = Guid.NewGuid().ToString();
            QueuedItemUuid = queuedItemUuid;
            Type = type;
            Result = result;

        }
    }
}
