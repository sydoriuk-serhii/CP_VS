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
        private NoteViewModel _viewModel;

        public ImportPage()
        {
            InitializeComponent();
            BindingContext = new NoteViewModel();
            _viewModel = (NoteViewModel)BindingContext;
        }

        private async void OnImportButtonClicked(object sender, EventArgs e)
        {
            string filePath = UrlEntry.Text;
            if (string.IsNullOrEmpty(filePath))
            {
                await DisplayAlert("Error", "Please enter a valid file path", "OK");
                return;
            }

            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(json);

                foreach (var note in notes)
                {
                    _viewModel.AddNote(note);
                }

                await DisplayAlert("Success", "Notes imported successfully", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to import notes: {ex.Message}", "OK");
            }
        }
    }
}

