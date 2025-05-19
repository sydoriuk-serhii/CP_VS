using Microsoft.Maui.Controls;
using Lab_1.ViewModels;
using System;

namespace Lab_1
{
    [QueryProperty(nameof(ItemId), "itemId")]
    public partial class ScheduleItemDetailsPage : ContentPage
    {
        private ScheduleViewModel _viewModel;
        private bool _isEditing;
        private int _itemId;

        public int ItemId
        {
            get => _itemId;
            set
            {
                _itemId = value;
                LoadScheduleItem();
            }
        }

        public ScheduleItemDetailsPage()
        {
            InitializeComponent();
            _viewModel = new ScheduleViewModel();
            BindingContext = _viewModel;
            _isEditing = false;
        }

        private async void LoadScheduleItem()
        {
            await _viewModel.LoadScheduleItemAsync(_itemId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _isEditing = false;
            UpdateEditState();
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            _isEditing = !_isEditing;
            UpdateEditState();
            EditButton.Text = _isEditing ? "Save" : "Edit";
            
            if (!_isEditing)
            {
                // Save changes when exiting edit mode
                _viewModel.SaveScheduleItemCommand.Execute(null);
            }
        }

        private void UpdateEditState()
        {
            // Update all fields to be editable or read-only based on editing state
            TitleEntry.IsReadOnly = !_isEditing;
            DayPicker.IsEnabled = _isEditing;
            StartTimePicker.IsEnabled = _isEditing;
            EndTimePicker.IsEnabled = _isEditing;
            LocationEntry.IsReadOnly = !_isEditing;
            DescriptionEditor.IsReadOnly = !_isEditing;
        }
    }
}