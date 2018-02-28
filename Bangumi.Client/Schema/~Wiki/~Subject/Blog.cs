namespace Bangumi.Client.Schema
{
    public class Blog
    {
        public int id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string image { get; set; }
        public int replies { get; set; }
        public int timestamp { get; set; }
        public string dateline { get; set; }
        public User user { get; set; }
    }
}
