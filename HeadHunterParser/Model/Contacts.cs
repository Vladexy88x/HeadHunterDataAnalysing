using System.Collections.Generic;

namespace HeadHunterParser.Model
{
    public class Contacts
    {
        public string email { get; set; }
        public string name { get; set; }
        public List<object> phones { get; set; }
    }
}
