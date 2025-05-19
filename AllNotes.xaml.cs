using Microsoft.Maui.Controls;
using Lab_1.ViewModels;
using Lab_1.Models;

namespace Lab_1
{
    [QueryProperty(nameof(NoteId), "noteId")]
    public partial class AllNotes : ContentPage
    {
        private NoteViewModel _viewModel;
        private bool _isDeleting;

        public int NoteId { get; set; }

        public AllNotes()
        {
            InitializeComponent();
            BindingContext = new NoteViewModel();
            _viewModel = (NoteViewModel)BindingContext;
            _isDeleting = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.RefreshNotes();
            await _viewModel.LoadNoteAsync(NoteId);
        }

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            _isDeleting = !_isDeleting;
            DeleteButton.Text = _isDeleting ? "Cancel Deleting" : "Delete";
        }

        private async void OnNoteSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Note selectedNote)
            {
                if (_isDeleting)
                {
                    _viewModel.SelectedNote = selectedNote;
                    _viewModel.DeleteNoteCommand.Execute(null);
                }
                else
                {
                    await Shell.Current.GoToAsync($"viewpage?noteId={selectedNote.Id}");
                }
            }
        }
        private async void OnImportButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("importpage");
        }

    }
}
