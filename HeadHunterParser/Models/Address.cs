using System.Collections.Generic;

namespace HeadHunterParser.Models
{
    public class Address
    {
        public string building { get; set; }
        public string city { get; set; }
        public object description { get; set; }
        public string id { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public object metro { get; set; }
        public List<object> metro_stations { get; set; }
        public object raw { get; set; }
        public string street { get; set; }

    }
}
