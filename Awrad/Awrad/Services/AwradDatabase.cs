using System.Collections.Generic;
using System.Threading.Tasks;
using Awrad.Models;
using SQLite;

namespace Awrad.Services
{
	public class AwradDatabase
	{
	    private readonly SQLiteAsyncConnection _sqLiteAsyncConnection;

		public AwradDatabase(string dbPath)
		{
			_sqLiteAsyncConnection = new SQLiteAsyncConnection(dbPath);
			//database.CreateTableAsync<ThikerClass>().Wait();
		}

		public Task<List<WirdClass>> GetAwradAsync()
		{
			return _sqLiteAsyncConnection.Table<WirdClass>().ToListAsync();
		}

		public Task<List<ThikerClass>> GetItemsNotDoneAsync()
		{
			return _sqLiteAsyncConnection.QueryAsync<ThikerClass>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}

        // Get a wird from DB
		public Task<WirdClass> GetWirdAsync(int id)
		{
            return _sqLiteAsyncConnection.Table<WirdClass>().Where(i => i.Id == id).FirstOrDefaultAsync();
		}

        // Get thiker from DB for a specific wird
        public Task<List<ThikerClass>> GetThikerAsync(int wirdType)
        {
            return _sqLiteAsyncConnection.Table<ThikerClass>().Where(i => i.WirdType == wirdType).ToListAsync();
        }

        // Get thiker from DB for a specific wird and size
        public Task<List<ThikerClass>> GetThikerAsync(int wirdType, int thikerSize)
        {
            return _sqLiteAsyncConnection.Table<ThikerClass>().Where(i => i.WirdType == wirdType &&
                                                            i.Size <= thikerSize).ToListAsync();
        }

        public async Task<List<ThikerClass>> GetRelatedThikerAsync(int wirdType, int thikerSize)
        {
            // Get the list of target wird
            var relatedThikerList = await _sqLiteAsyncConnection.Table<RelatedThikerClass>().Where(i => i.Wird == wirdType).ToListAsync();

            // Iterate through the different wird and get the target thiker
            var outputThikerList = new List<ThikerClass>();
            foreach (var relatedThiker in relatedThikerList)
            {
                // Get list of thiker with a specific size
                var thikerList = await GetThikerAsync(relatedThiker.RelatedWird, relatedThiker.ThikerSize);
                var thiker = thikerList[relatedThiker.CurrentThiker % thikerList.Count];
                
                // Add title to thiker content
                thiker.Content = relatedThiker.Description + "\n" + thiker.Content;

                // Get the current thiker from the list and add to output
                outputThikerList.Add(thiker);

                // Update current thiker counter in related thiker
                relatedThiker.CurrentThiker++;
                await SaveRelatedThikerAsync(relatedThiker);
            }

            return outputThikerList;
        }

        public Task<int> SaveRelatedThikerAsync(RelatedThikerClass relatedThiker)
        {
            if (relatedThiker.Id != 0)
            {
                return _sqLiteAsyncConnection.UpdateAsync(relatedThiker);
            }
            else
            {
                return _sqLiteAsyncConnection.InsertAsync(relatedThiker);
            }
        }

        public Task<int> SaveWirdAsync(WirdClass wird)
        {
            if (wird.Id != 0)
            {
                return _sqLiteAsyncConnection.UpdateAsync(wird);
            }
            else
            {
                return _sqLiteAsyncConnection.InsertAsync(wird);
            }
        }

        public Task<int> SaveThikerAsync(ThikerClass item)
		{
			if (item.Id != 0)
			{
				return _sqLiteAsyncConnection.UpdateAsync(item);
			}
			else {
				return _sqLiteAsyncConnection.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(ThikerClass item)
		{
			return _sqLiteAsyncConnection.DeleteAsync(item);
		}
	}
}

