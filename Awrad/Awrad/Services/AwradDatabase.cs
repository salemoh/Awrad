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

