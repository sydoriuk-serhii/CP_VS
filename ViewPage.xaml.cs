using Microsoft.Maui.Controls;
using Lab_1.ViewModels;
using System.ComponentModel.Design.Serialization;

namespace Lab_1
{
    [QueryProperty(nameof(NoteId), "noteId")]
    public partial class ViewPage : ContentPage
    {
        private NoteViewModel _viewModel;
        private bool _isEditing;

        public int NoteId { get; set; }

        public ViewPage()
        {
            InitializeComponent();
            BindingContext = new NoteViewModel();
            _viewModel = (NoteViewModel)BindingContext;
            _isEditing = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadNoteAsync(NoteId);
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            ContentStack.Children.Clear();

            var titleEntry = new Entry
            {
                Text = _viewModel.SelectedNote.Title,
                IsReadOnly = !_isEditing
            };
            titleEntry.SetBinding(Entry.TextProperty, new Binding("SelectedNote.Title", BindingMode.TwoWay));
            if (_isEditing)
            {
                titleEntry.TextChanged += OnTextChanged;
            }

            var contentEditor = new Editor
            {
                Text = _viewModel.SelectedNote.Description,
                VerticalOptions = LayoutOptions.FillAndExpand,
                IsReadOnly = !_isEditing
            };
            contentEditor.SetBinding(Editor.TextProperty, new Binding("SelectedNote.Content", BindingMode.TwoWay));
            if (_isEditing)
            {
                contentEditor.TextChanged += OnTextChanged;
            }

            ContentStack.Children.Add(titleEntry);
            ContentStack.Children.Add(contentEditor);

            ((Button)sender).Text = _isEditing ? "View" : "Edit";

            _isEditing = !_isEditing;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SaveNoteCommand.Execute(null);
        }
    }
}
