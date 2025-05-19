using Microsoft.Maui.Controls;
using Lab_1.ViewModels;
using Lab_1.Models;
using System;

namespace Lab_1
{
    [QueryProperty(nameof(Day), "day")]
    public partial class DaySchedulePage : ContentPage
    {
        private ScheduleViewModel _viewModel;
        private bool _isDeleting;
        private string _day;

        public string Day
        {
            get => _day;
            set
            {
                _day = value;
                UpdateSelectedDay();
            }
        }

        public string DayTitle => Day;

        public DaySchedulePage()
        {
            InitializeComponent();
            _viewModel = new ScheduleViewModel();
            BindingContext = _viewModel;
            _isDeleting = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateSelectedDay();
            _viewModel.RefreshSchedule();
        }

        private void UpdateSelectedDay()
        {
            if (string.IsNullOrEmpty(_day))
                return;

            if (Enum.TryParse(_day, true, out DayOfWeek dayOfWeek))
            {
                _viewModel.SelectedDay = dayOfWeek;
                Title = _day; // Update the page title
            }
        }

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            _isDeleting = !_isDeleting;
            DeleteButton.Text = _isDeleting ? "Cancel" : "Delete";
        }

        private async void OnScheduleItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ScheduleItem selectedItem)
            {
                if (_isDeleting)
                {
                    _viewModel.SelectedItem = selectedItem;
                    _viewModel.DeleteScheduleItemCommand.Execute(null);
                    _isDeleting = false;
                    DeleteButton.Text = "Delete";
                }
                else
                {
                    await Shell.Current.GoToAsync($"itemdetails?itemId={selectedItem.Id}");
                }
            } 
        }

        private async void OnImportButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("importpage");
        }
    }
}