using Microsoft.Maui.Controls;

namespace Lab_1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("viewpage", typeof(ViewPage));
            Routing.RegisterRoute("allnotes", typeof(AllNotes));
            Routing.RegisterRoute("importpage", typeof(ImportPage)); // Register ImportPage route
        }
    }
}
