using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Constants.LocalDatabase.Tables
{
    public class CatchSpeciesTable 
    {
        SQLiteAsyncConnection Database;
        public CatchSpeciesTable() { }
        public async Task AddSpeciesAsync()
        {
            var SelectionCollection = new ObservableCollection<DBSpeciesObject>
            {
                new DBSpeciesObject
                {
                    Species = "Albacore Tuna",
                    ScientificName = "Thunnus alalunga",
                    IsActive = true,
                    IsETP = false,
                },
                new DBSpeciesObject
                {
                    Species = "Bigeye Tuna",
                    ScientificName = "Thunnus obesus",
                    IsActive = true,
                    IsETP = false,
                },
                new DBSpeciesObject
                {
                    Species = "Northern Bluefin Tuna",
                    ScientificName = "Thunnus thynnus",
                    IsActive = true,
                    IsETP = false,
                },
                new DBSpeciesObject
                {
                    Species = "Yellowfin Tuna",
                    ScientificName = "Thunnus albacares",
                    IsActive = true,
                    IsETP = false,
                },
                
                //ETP Species
                new DBSpeciesObject
                {
                    Species = "Whale Shark",
                    ScientificName = "Rhincodon typus",
                    IsActive = true,
                    IsETP = true,
                },

                 new DBSpeciesObject
                {
                    Species = "Turtle",
                    ScientificName = "",
                    IsActive = true,
                    IsETP = true,
                },

                   new DBSpeciesObject
                {
                    Species = "Birds",
                    ScientificName = "",
                    IsActive = true,
                    IsETP = true,
                },
            };

            foreach (DBSpeciesObject obj in SelectionCollection)
            {
                SaveItemAsync(obj);
            }

            Preferences.Set(Pref.CATCH_SPECIES_LIST, JsonConvert.SerializeObject(SelectionCollection));
        }
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseClass.DatabasePath, DatabaseClass.Flags);
            var result = await Database.CreateTableAsync<DBSpeciesObject>();

        }
        public async Task<List<DBSpeciesObject>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<DBSpeciesObject>().ToListAsync();
        }
        public async Task<DBSpeciesObject> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<DBSpeciesObject>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(DBSpeciesObject item)
        {
            await Init();
            if (item.Id != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(DBSpeciesObject item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }
    }
}
