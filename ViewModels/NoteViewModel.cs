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

namespace Lab_1.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private NotesDatabase _database;
        private Note _selectedNote;

        public ObservableCollection<Note> Notes { get; set; }
        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (_selectedNote != value)
                {
                    _selectedNote = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand SaveNoteCommand { get; }
        public ICommand EditNoteCommand { get; }

        public NoteViewModel()
        {
            _database = new NotesDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
            Notes = new ObservableCollection<Note>();
            LoadNotes();

            AddNoteCommand = new Command(AddNote);
            DeleteNoteCommand = new Command(DeleteNote);
            SaveNoteCommand = new Command(SaveNote);
        }

        private async void LoadNotes()
        {
            var notes = await _database.GetNotesAsync();
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }

        public async Task LoadNoteAsync(int noteId)
        {
            SelectedNote = await _database.GetNoteAsync(noteId);
        }

        public void AddNote(Note existingNote)
        {
            Notes.Add(existingNote);
            SelectedNote = existingNote;
        }

        private void AddNote()
        {
            var newNote = new Note { Title = "New Note", Content = $"Model: {DeviceInfo.Model}\n" +
                $"Manufacturer: {DeviceInfo.Manufacturer}\n" +
                $"Platform: {DeviceInfo.Platform}\n" +
                $"OS Version: {DeviceInfo.VersionString}\n" };
            Notes.Add(newNote);
            SelectedNote = newNote;
            SaveNotes();
        }

        private async void DeleteNote()
        {
            if (SelectedNote != null)
            {
                await _database.DeleteNoteAsync(SelectedNote);
                Notes.Remove(SelectedNote);
                SelectedNote = null;
                SaveNotes();
            }
        }

        private async void SaveNote()
        {
            if (SelectedNote != null)
            {
                await _database.SaveNoteAsync(SelectedNote);
            }
        }

        private async void SaveNotes()
        {
            foreach (var note in Notes)
            {
                await _database.SaveNoteAsync(note);
            }
        }

        public void RefreshNotes()
        {
            LoadNotes();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(SelectedNote))
            {
                SaveNote();
            }
        }
    }

}
