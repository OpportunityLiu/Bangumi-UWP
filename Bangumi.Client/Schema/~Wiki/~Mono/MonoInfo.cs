namespace Bangumi.Client.Schema
{
    public class MonoInfo
    {
        public string birth { get; set; }
        public string height { get; set; }
        public string gender { get; set; }
        public Alias alias { get; set; }
        // string or string[]
        public object source { get; set; }
        public string name_cn { get; set; }
        public string cv { get; set; }
    }
}
