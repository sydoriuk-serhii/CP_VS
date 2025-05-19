using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CP.Models;
using CP.Services;

namespace CP.ViewModels
{
    public class DayScheduleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ScheduleItem> Items { get; set; } = new();
        private readonly ScheduleService _service = new();
        public string Day { get; }

        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }

        public DayScheduleViewModel(string day)
        {
            Day = day;
            AddCommand = new Command(AddNewItem);
            SaveCommand = new Command(async () => await SaveAsync());
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var items = await _service.LoadScheduleAsync(Day);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Items.Clear();
                foreach (var item in items)
                    Items.Add(item);
            });
        }

        private async Task SaveAsync()
        {
            await _service.SaveScheduleAsync(Day, Items.ToList());
        }

        private void AddNewItem()
        {
            Items.Add(new ScheduleItem
            {
                Title = "Нова подія",
                Description = "",
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(10),
                Location = ""
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}