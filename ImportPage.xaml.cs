using Microsoft.Maui.Controls;
using Lab_1.ViewModels;
using Lab_1.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lab_1
{
    public partial class ImportPage : ContentPage
    {
        private ScheduleViewModel _viewModel;

        public ImportPage()
        {
            InitializeComponent();
            BindingContext = new ScheduleViewModel();
            _viewModel = (ScheduleViewModel)BindingContext;
        }

        private async void OnImportButtonClicked(object sender, EventArgs e)
        {
            string filePath = FilePathEntry.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                await DisplayAlert("Error", "Please enter a valid file path", "OK");
                return;
            }

            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<ScheduleItem> items = JsonConvert.DeserializeObject<List<ScheduleItem>>(json);

                foreach (var item in items)
                {
                    await _viewModel.SaveScheduleItemAsync(item);
                }

                _viewModel.RefreshSchedule();
                await DisplayAlert("Success", $"{items.Count} schedule items imported successfully", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to import schedule: {ex.Message}", "OK");
            }
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}