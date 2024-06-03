using MrG.AI.Client.Database.Data;
using MrG.AI.Client.Enum;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.Database
{
    public class DatabaseHelper : IDatabase
    {
        public static IDatabase instance;
        public static IDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseHelper();
                }
                return instance;
            }
        }

        private SQLiteAsyncConnection Database;

        public DatabaseHelper()
        {
            instance = this;
            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            Database.CreateTableAsync<Request>();
            Database.CreateTableAsync<RunItem>();
            Database.CreateTableAsync<OutputItem>();

        }

        public const string DatabaseFilename = "MrG.db";

        public const SQLiteOpenFlags Flags =
           // open the database in read/write mode
           SQLiteOpenFlags.ReadWrite |
           // create the database if it doesn't exist
           SQLiteOpenFlags.Create |
           // enable multi-threaded database access
           SQLiteOpenFlags.SharedCache;

        private static string DatabasePath => Path.Combine(DatabaseDirectory, DatabaseFilename);

        public static string DatabaseDirectory = null;
        public async Task<List<T>> GetAll<T>(Expression<Func<T, bool>> func) where T : BaseTable, new()
        {
            //make from func a linq expression

            return await Database.Table<T>().Where(func).ToListAsync();
        }

        public async Task<Tuple<List<T>, int>> GetAll<T>(Expression<Func<T, bool>> func, int start, int end, bool orderDesc) where T : BaseTable, new()
        {
            var query = Database.Table<T>().Where(func);
            if (orderDesc)
            {
                query = query.OrderByDescending(i => i.CreatedAt);
            }
            else
            {
                query = query.OrderBy(i => i.CreatedAt);
            }
            var count = await query.CountAsync();
            var result = await query.Skip(start).Take(end - start).ToListAsync();
            return new Tuple<List<T>, int>(result, count);
        }

        public async Task<Tuple<List<T>, int>> GetAll<T>(int start, int end, bool orderDesc) where T : BaseTable, new()
        {
            var query = Database.Table<T>();
            if (orderDesc)
            {
                query = query.OrderByDescending(i => i.CreatedAt);
            }
            else
            {
                query = query.OrderBy(i => i.CreatedAt);
            }
            var count = await query.CountAsync();
            var result = await query.Skip(start).Take(end - start).ToListAsync();
            return new Tuple<List<T>, int>(result, count);
        }

        public async Task<List<T>> GetAll<T>() where T : BaseTable, new()
        {
            return await Database.Table<T>().ToListAsync();
        }
        public async Task<T> Get<T>(string uuid) where T : BaseTable, new()
        {
            return await Database.Table<T>().Where(i => i.Uuid == uuid).FirstOrDefaultAsync();
        }
        public async Task<int> Save<T>(T item) where T : BaseTable, new()
        {
            item.UpdatedAt = DateTime.Now;
            if (item.Uuid != null)
            {
                if (await Get<T>(item.Uuid) != null)
                    return await Database.UpdateAsync(item);
            }
            return await Database.InsertAsync(item);
        }



        public async Task<int> UpdateStatus(string uuid, RequestStatus requestStatus)
        {
            var request = await Get<Request>(uuid);
            request.RequestStatus = requestStatus;
            return await Save(request);
        }



    }
}
