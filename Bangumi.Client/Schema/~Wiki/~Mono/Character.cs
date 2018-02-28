namespace Bangumi.Client.Schema
{
    public class Character : MonoBase
    {
        public string name_cn { get; set; }
        public string role_name { get; set; }
        public int comment { get; set; }
        public int collects { get; set; }
        public MonoInfo info { get; set; }
        public Person[] actors { get; set; }
    }
}
