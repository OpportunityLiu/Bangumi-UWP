namespace Bangumi.Client.Schema
{
    public class Topic
    {
        public int id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public int main_id { get; set; }
        public int timestamp { get; set; }
        public int lastpost { get; set; }
        public int replies { get; set; }
        public User user { get; set; }
    }
}
