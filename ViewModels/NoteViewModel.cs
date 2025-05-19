using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Lab_1.Models;
using Lab_1.Services;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Lab_1.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        private ScheduleDatabase _database;
        private ScheduleItem _selectedItem;
        private DayOfWeek _selectedDay;

        public ObservableCollection<ScheduleItem> AllScheduleItems { get; set; }
        public ObservableCollection<ScheduleItem> FilteredScheduleItems { get; set; }
        
        public ScheduleItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public DayOfWeek SelectedDay
        {
            get => _selectedDay;
            set
            {
                if (_selectedDay != value)
                {
                    _selectedDay = value;
                    FilterScheduleItems();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddScheduleItemCommand { get; }
        public ICommand DeleteScheduleItemCommand { get; }
        public ICommand SaveScheduleItemCommand { get; }
        public ICommand ChangeDayCommand { get; }

        public ScheduleViewModel()
        {
            _database = new ScheduleDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Schedule.db3"));
            AllScheduleItems = new ObservableCollection<ScheduleItem>();
            FilteredScheduleItems = new ObservableCollection<ScheduleItem>();
            _selectedDay = DateTime.Now.DayOfWeek; // Default to current day
            
            LoadScheduleItems();

            AddScheduleItemCommand = new Command(AddScheduleItem);
            DeleteScheduleItemCommand = new Command(DeleteScheduleItem);
            SaveScheduleItemCommand = new Command(SaveScheduleItem);
            ChangeDayCommand = new Command<DayOfWeek>(day => SelectedDay = day);
        }

        private async void LoadScheduleItems()
        {
            var items = await _database.GetScheduleItemsAsync();
            AllScheduleItems.Clear();
            foreach (var item in items)
            {
                AllScheduleItems.Add(item);
            }
            FilterScheduleItems();
        }
        
        private void FilterScheduleItems()
        {
            FilteredScheduleItems.Clear();
            var filteredItems = AllScheduleItems.Where(item => item.DayOfWeek == SelectedDay)
                                               .OrderBy(item => item.StartTime);
            foreach (var item in filteredItems)
            {
                FilteredScheduleItems.Add(item);
            }
        }

        public async Task LoadScheduleItemAsync(int itemId)
        {
            SelectedItem = await _database.GetScheduleItemAsync(itemId);
        }

        private void AddScheduleItem()
        {
            var newItem = new ScheduleItem 
            { 
                Title = "New Schedule Item", 
                DayOfWeek = SelectedDay,
                StartTime = new TimeSpan(9, 0, 0), // Default 9:00 AM
                EndTime = new TimeSpan(10, 0, 0),  // Default 10:00 AM
                Description = "Add description here",
                Location = "Location"
            };
            
            AllScheduleItems.Add(newItem);
            FilteredScheduleItems.Add(newItem);
            SelectedItem = newItem;
            SaveScheduleItems();
        }

        private async void DeleteScheduleItem()
        {
            if (SelectedItem != null)
            {
                await _database.DeleteScheduleItemAsync(SelectedItem);
                AllScheduleItems.Remove(SelectedItem);
                FilteredScheduleItems.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        private async void SaveScheduleItem()
        {
            if (SelectedItem != null)
            {
                await _database.SaveScheduleItemAsync(SelectedItem);
                // Refresh the filtered items in case day changed
                FilterScheduleItems();
            }
        }

        private async void SaveScheduleItems()
        {
            foreach (var item in AllScheduleItems)
            {
                await _database.SaveScheduleItemAsync(item);
            }
        }

        public void RefreshSchedule()
        {
            LoadScheduleItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(SelectedItem))
            {
                SaveScheduleItem();
            }
        }
    }
}