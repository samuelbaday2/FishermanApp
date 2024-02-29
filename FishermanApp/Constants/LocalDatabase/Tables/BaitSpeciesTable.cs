using FishermanApp.Objects;
using FishermanApp.Objects.DbObjects;
using GoogleGson.Annotations;
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
    public class BaitSpeciesTable
    {
        SQLiteAsyncConnection Database;

        public BaitSpeciesTable()
        {
            
        }
        public async Task AddSpeciesAsync() {
            var SelectionCollection = new ObservableCollection<DBSpeciesObject>
            {
                new DBSpeciesObject
                {
                    BaitSpecies = "Bigeye Scad",
                    ScientificName = " Selar crumenophthalmus",
                    IsActive = true,
                },
                new DBSpeciesObject
                {
                    BaitSpecies = "Flying Fish",
                    ScientificName = "Hirundichthys affinis",
                    IsActive = true,

                },
                new DBSpeciesObject
                {
                    BaitSpecies = "Thread Herring",
                    ScientificName = "Opisthonema oglinum",
                    IsActive = true,

                },
                new DBSpeciesObject
                {
                    BaitSpecies = "Ballyhoo",
                    ScientificName = "Hemiramphus brasiliensis",
                    IsActive = true,

                },
                new DBSpeciesObject
                {
                    BaitSpecies = "Round Scad",
                    ScientificName = "Decapterus punctatus",
                    IsActive = true,

                },
                new DBSpeciesObject
                {
                    BaitSpecies = "Diamondback Squid",
                    ScientificName = "Thysanoteuthis rhombus",
                    IsActive = true,

                },
                new DBSpeciesObject
                {
                    BaitSpecies = "Humboldt squid",
                    ScientificName = "Dosidicus gigas",
                    IsActive = true,
                }
            };

            foreach (DBSpeciesObject obj in SelectionCollection)
            {
                SaveItemAsync(obj);
            }

            Preferences.Set(Pref.SPECIES_LIST, JsonConvert.SerializeObject(SelectionCollection));
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
