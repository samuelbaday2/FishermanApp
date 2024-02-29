using FishermanApp.Objects.DbObjects;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class TripTable
    {
        SQLiteAsyncConnection Database;

        public TripTable()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DbTripObject>();
        }
        public async Task<List<DbTripObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DbTripObject>().ToListAsync();
        }

        //public async Task<List<DbTripObject>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await Database.Table<DbTripObject>().Where(t => t.Done).ToListAsync();

        //    // SQL queries are also possible
        //    //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public async Task<DbTripObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DbTripObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DbTripObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DbTripObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
