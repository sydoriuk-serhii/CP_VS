using CP.ViewModels;

namespace CP.Views
{
    public partial class DaySchedulePage : ContentPage
    {
        public DaySchedulePage(string day)
        {
            InitializeComponent();
            BindingContext = new DayScheduleViewModel(day);
        }
    }
}