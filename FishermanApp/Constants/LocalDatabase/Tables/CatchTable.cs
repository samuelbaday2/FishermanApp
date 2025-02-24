using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class CatchTable
    {
        SQLiteAsyncConnection Database;

        public CatchTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBCatchObject>();
        }
        public async Task<List<DBCatchObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBCatchObject>().ToListAsync();
        }
        public async Task<bool> GetEtpItemsAsync(int setId)
        {
            await Init();
            var list = await Database.Table<DBCatchObject>().Where(x => x.IsETP && x.TripId == setId).ToListAsync();
            return list.Count > 0;
        }
        public async Task<List<DBCatchObject>> GetEtpItemsListAsync(int setId)
        {
            await Init();
            var list = await Database.Table<DBCatchObject>().Where(x => x.IsETP && x.TripId == setId).ToListAsync();
            return list;
        }
        public async Task<List<DBCatchObject>> GetItemsBySetIdAsync(int setId)
        {
            await Init();
            return await Database.Table<DBCatchObject>().Where(x => x.SetId == setId).ToListAsync();
        }
        public async Task<DBCatchObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBCatchObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DBCatchObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBCatchObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
