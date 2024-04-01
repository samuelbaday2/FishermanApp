using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class TripSetTable
    {
        SQLiteAsyncConnection Database;

        public TripSetTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBSetObject>();
        }
        public async Task<List<DBSetObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBSetObject>().ToListAsync();
        }
        public async Task<DBSetObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBSetObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<DBSetObject> GetItemByTripIdAsync(int id)
        {
            await Init();
            return await Database.Table<DBSetObject>().Where(i => i.TripId == id).FirstOrDefaultAsync();
        }
        public async Task<List<DBSetObject>> GetItesmByTripIdAsync(int id)
        {
            await Init();
            return await Database.Table<DBSetObject>().Where(i => i.TripId == id).ToListAsync();
        }

        public async Task<int> SaveItemAsync(DBSetObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBSetObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
