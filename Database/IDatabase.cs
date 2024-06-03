using MrG.AI.Client.Database.Data;
using MrG.AI.Client.Enum;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.Database
{
    public interface IDatabase
    {
        Task<Tuple<List<T>, int>> GetAll<T>(Expression<Func<T, bool>> func, int start, int end, bool orderDesc) where T : BaseTable, new();
        Task<Tuple<List<T>, int>> GetAll<T>(int start, int end, bool orderDesc) where T : BaseTable, new();
        Task<List<T>> GetAll<T>(Expression<Func<T, bool>> func) where T : BaseTable, new();

       Task<List<T>> GetAll<T>() where T : BaseTable, new();
        Task<T> Get<T>(string uuid) where T : BaseTable, new();
        Task<int> Save<T>(T item) where T : BaseTable, new();
        Task<int> UpdateStatus(string uuid, RequestStatus requestStatus);
    }
}
