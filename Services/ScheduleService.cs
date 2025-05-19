using System.Text.Json;
using CP.Models;

namespace CP.Services
{
    public class ScheduleService
    {
        private const string FileName = "schedule.json";
        private string FilePath => Path.Combine(FileSystem.AppDataDirectory, FileName);

        public async Task<List<ScheduleItem>> LoadScheduleAsync(string day)
        {
            if (!File.Exists(FilePath))
                return new List<ScheduleItem>();

            using var stream = File.OpenRead(FilePath);
            var allData = await JsonSerializer.DeserializeAsync<Dictionary<string, List<ScheduleItem>>>(stream)
                          ?? new Dictionary<string, List<ScheduleItem>>();

            return allData.TryGetValue(day, out var items) ? items : new List<ScheduleItem>();
        }

        public async Task SaveScheduleAsync(string day, List<ScheduleItem> schedule)
        {
            Dictionary<string, List<ScheduleItem>> allData;

            if (File.Exists(FilePath))
            {
                using var stream = File.OpenRead(FilePath);
                allData = await JsonSerializer.DeserializeAsync<Dictionary<string, List<ScheduleItem>>>(stream)
                           ?? new Dictionary<string, List<ScheduleItem>>();
            }
            else
            {
                allData = new Dictionary<string, List<ScheduleItem>>();
            }

            allData[day] = schedule;

            using var writeStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(writeStream, allData, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}