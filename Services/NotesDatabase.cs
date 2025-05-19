using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab_1.Models;

namespace Lab_1.Services
{
    public class ScheduleDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ScheduleDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ScheduleItem>().Wait();
        }

        public Task<List<ScheduleItem>> GetScheduleItemsAsync()
        {
            return _database.Table<ScheduleItem>().ToListAsync();
        }

        public Task<List<ScheduleItem>> GetScheduleItemsByDayAsync(DayOfWeek day)
        {
            return _database.Table<ScheduleItem>()
                .Where(i => i.DayOfWeek == day)
                .OrderBy(i => i.StartTime)
                .ToListAsync();
        }

        public Task<ScheduleItem> GetScheduleItemAsync(int id)
        {
            return _database.Table<ScheduleItem>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveScheduleItemAsync(ScheduleItem item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteScheduleItemAsync(ScheduleItem item)
        {
            return _database.DeleteAsync(item);
        }
    }
}