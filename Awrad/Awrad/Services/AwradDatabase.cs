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
			database.CreateTableAsync<Thiker>().Wait();
		}

		public Task<List<Thiker>> GetItemsAsync()
		{
			return database.Table<Thiker>().ToListAsync();
		}

		public Task<List<Thiker>> GetItemsNotDoneAsync()
		{
			return database.QueryAsync<Thiker>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
		}

		public Task<Thiker> GetItemAsync(int id)
		{
			return database.Table<Thiker>().Where(i => i.Id == id).FirstOrDefaultAsync();
		}

		public Task<int> SaveItemAsync(Thiker item)
		{
			if (item.Id != 0)
			{
				return database.UpdateAsync(item);
			}
			else {
				return database.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(Thiker item)
		{
			return database.DeleteAsync(item);
		}
	}
}

