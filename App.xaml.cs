using Microsoft.Maui.Controls;

namespace Lab_1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
