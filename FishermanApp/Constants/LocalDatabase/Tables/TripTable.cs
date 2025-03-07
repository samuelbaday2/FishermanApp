﻿using FishermanApp.Objects.DbObjects;
using Newtonsoft.Json;
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
        public async Task RunQuery() {
            await Init();
            try
            {
                var query = await Database.QueryAsync<DbTripObject>("select * from DbTripObject");
                DbTripObject dbTripObject = query.FirstOrDefault();
                Console.WriteLine($"QUERY RESULT : {JsonConvert.SerializeObject(dbTripObject)}");
            }
            catch (Exception e)
            {
            }
          
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
        public async Task<List<DbTripObject>> GetUnuploadedData()
        {
            await Init();
            return await Database.Table<DbTripObject>().Where(x => x.IsTripEnded == true && x.IsUploaded == false && x.IsActive == true).ToListAsync();
        }
        public async Task<List<DbTripObject>> GetItemsAsync(DateTime dateTime, DateTime dateTimeMax)
        {
            await Init();
            DateTime range = dateTimeMax.AddHours(23).AddMinutes(59).AddSeconds(59);
            return await Database.Table<DbTripObject>().Where(i => i.RecordedOn >= dateTime && i.RecordedOn <= range).ToListAsync();
        }
        //public async Task<List<DbTripObject>> GetItemsNotDoneAsync()
        //{
        //    await Init();
        //    return await Database.Table<DbTripObject>().Where(t => t.Done).ToListAsync();

        //    // SQL queries are also possible
        //    //return await Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}
        public async Task<DbTripObject> GetFirstItemAsync()
        {
            await Init();
            return await Database.Table<DbTripObject>().FirstOrDefaultAsync();
        }
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
