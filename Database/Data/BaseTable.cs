using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MrG.AI.Client.Database.Data
{
    public class BaseTable : ObservableObject
    {
        [PrimaryKey]
        public string Uuid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BaseTable()
        {
            Uuid = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}
