namespace CP.Models
{
    public class ScheduleItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }
    }
}