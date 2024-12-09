using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class HACCPTable
    {
        SQLiteAsyncConnection Database;

        public HACCPTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBHACCPObject>();
        }
        public async Task<List<DBHACCPObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBHACCPObject>().ToListAsync();
        }
        public async Task<List<DBHACCPObject>> GetItemsBySetIdAsync(int setId)
        {
            await Init();
            return await Database.Table<DBHACCPObject>().Where(x => x.Id == setId).ToListAsync();
        }
        public async Task<DBHACCPObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBHACCPObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DBHACCPObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBHACCPObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
