using CP.Views;

namespace CP.Views
{
    public partial class DaySelectorPage : ContentPage
    {
        public DaySelectorPage()
        {
            InitializeComponent();
        }

        private async void OnDayClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string day)
            {
                await Navigation.PushAsync(new DaySchedulePage(day));
            }
        }
    }
}