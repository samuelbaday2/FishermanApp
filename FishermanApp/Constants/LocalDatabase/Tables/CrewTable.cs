using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class CrewTable
    {
        SQLiteAsyncConnection Database;

        public CrewTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBCrewObject>();
        }
        public async Task<List<DBCrewObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBCrewObject>().ToListAsync();
        }
        public async Task<List<DBCrewObject>> GetItemsBySetIdAsync(int setId)
        {
            await Init();
            return await Database.Table<DBCrewObject>().Where(x => x.Id == setId).ToListAsync();
        }
        public async Task<DBCrewObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBCrewObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DBCrewObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBCrewObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
