using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class TrackingTable
    {
        SQLiteAsyncConnection Database;

        public TrackingTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBTrackingTable>();
        }
        public async Task<List<DBTrackingTable>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBTrackingTable>().ToListAsync();
        }
        public async Task<List<DBTrackingTable>> GetItemsBySetIdAsync(int setId)
        {
            await Init();
            return await Database.Table<DBTrackingTable>().Where(x => x.TripId == setId).ToListAsync();
        }
        public async Task<DBTrackingTable> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBTrackingTable>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DBTrackingTable item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBTrackingTable item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
