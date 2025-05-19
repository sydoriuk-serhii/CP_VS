using Microsoft.Maui.Controls;

namespace Lab_1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Register routes for navigation
            Routing.RegisterRoute("itemdetails", typeof(ScheduleItemDetailsPage));
            Routing.RegisterRoute("monday", typeof(DaySchedulePage));
            Routing.RegisterRoute("tuesday", typeof(DaySchedulePage));
            Routing.RegisterRoute("wednesday", typeof(DaySchedulePage));
            Routing.RegisterRoute("thursday", typeof(DaySchedulePage));
            Routing.RegisterRoute("friday", typeof(DaySchedulePage));
            Routing.RegisterRoute("saturday", typeof(DaySchedulePage));
            Routing.RegisterRoute("sunday", typeof(DaySchedulePage));
            Routing.RegisterRoute("importpage", typeof(ImportPage));
        }
    }
}