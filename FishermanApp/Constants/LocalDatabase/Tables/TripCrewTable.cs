using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class TripCrewTable
    {
        SQLiteAsyncConnection Database;

        public TripCrewTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBTripCrewObject>();
        }
        public async Task<List<DBTripCrewObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBTripCrewObject>().ToListAsync();
        }
        public async Task<List<DBTripCrewObject>> GetItemsByTripIdAsync(int tripId)
        {
            await Init();
            return await Database.Table<DBTripCrewObject>().Where(x => x.TripId == tripId).ToListAsync();
        }
        public async Task<DBTripCrewObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBTripCrewObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DBTripCrewObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBTripCrewObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
