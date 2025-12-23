namespace EventProject.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime EventDateTime { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
    }
}
