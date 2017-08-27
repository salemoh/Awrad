using System.Collections.Generic;
using System.Threading.Tasks;
using Awrad.Models;
using SQLite;

namespace Awrad.Services
{
	public class AwradDatabase
	{
		readonly SQLiteAsyncConnection database;

		public AwradDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
			//database.CreateTableAsync<ThikerClass>().Wait();
		}

		public Task<List<WirdClass>> GetAwradAsync()
		{
			return database.Table<WirdClass>().ToListAsync();
		}

		public Task<List<ThikerClass>> GetItemsNotDoneAsync()
		{
			return database.QueryAsync<ThikerClass>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}

        // Get a wird from DB
		public Task<WirdClass> GetWirdAsync(int id)
		{
            return database.Table<WirdClass>().Where(i => i.Id == id).FirstOrDefaultAsync();
		}

        // Get thiker from DB for a specific wird
        public Task<List<ThikerClass>> GetThikerAsync(int wirdType)
        {
            return database.Table<ThikerClass>().Where(i => i.WirdType == wirdType).ToListAsync();
        }

        // Get thiker from DB for a specific wird and size
        public Task<List<ThikerClass>> GetThikerAsync(int wirdType, int thikerSize)
        {
            return database.Table<ThikerClass>().Where(i => i.WirdType == wirdType &&
                                                            i.Size == thikerSize).ToListAsync();
        }

        public async Task<List<ThikerClass>> GetRelatedThikerAsync(int wirdType, int thikerSize)
        {
            // Get the list of target wird
            var relatedThikerList = await database.Table<RelatedThikerClass>().Where(i => i.Wird == wirdType).ToListAsync();

            // Iterate through the different wird and get the target thiker
            var outputThikerList = new List<ThikerClass>();
            foreach (var relatedThiker in relatedThikerList)
            {
                // Get list of thiker with a specific size
                var thikerList = await GetThikerAsync(relatedThiker.RelatedWird, relatedThiker.ThikerSize);

                // Get the current thiker from the list and add to output
                outputThikerList.Add(thikerList[relatedThiker.CurrentThiker % thikerList.Count]);

                // Update current thiker counter in related thiker
                relatedThiker.CurrentThiker = (relatedThiker.CurrentThiker + 1) % thikerList.Count;
                await SaveRelatedThikerAsync(relatedThiker);
            }

            return outputThikerList;
        }

        public Task<int> SaveRelatedThikerAsync(RelatedThikerClass relatedThiker)
        {
            if (relatedThiker.Id != 0)
            {
                return database.UpdateAsync(relatedThiker);
            }
            else
            {
                return database.InsertAsync(relatedThiker);
            }
        }

        public Task<int> SaveWirdAsync(WirdClass wird)
        {
            if (wird.Id != 0)
            {
                return database.UpdateAsync(wird);
            }
            else
            {
                return database.InsertAsync(wird);
            }
        }

        public Task<int> SaveThikerAsync(ThikerClass item)
		{
			if (item.Id != 0)
			{
				return database.UpdateAsync(item);
			}
			else {
				return database.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(ThikerClass item)
		{
			return database.DeleteAsync(item);
		}
	}
}

