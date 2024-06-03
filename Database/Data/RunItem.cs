using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.Database.Data
{
    public class RunItem : BaseTable
    {

        public string RequestUuid { get; set; }

        public string QueueStatus { get; set; }




        public RunItem() : this(string.Empty, string.Empty)
        {
        }

        public RunItem(string requestUuid, string queueStatus) : base()
        {
            RequestUuid = requestUuid;
            QueueStatus = queueStatus;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public async Task<List<OutputItem>> GetOutputItems()
        {
            if(DatabaseHelper.Instance == null)
            {
                return new List<OutputItem>();
            }
            return await DatabaseHelper.Instance.GetAll<OutputItem>(x => x.QueuedItemUuid == Uuid);
        }

    }
}
