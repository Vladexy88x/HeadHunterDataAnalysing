using System.Collections.Generic;

namespace HeadHunterParser.Model
{
    class Item
    {
        public Address Address { get; set; }
        public string alternate_url { get; set; }
        public string apply_alternate_url { get; set; }
        public bool? archived { get; set; }
        public Area Area { get; set; }
        public Contacts Contacts { get; set; }
        public string created_at { get; set; }
        public object department { get; set; }
        public Employer Employer { get; set; }
        public bool? has_test { get; set; }
        public string id { get; set; }
        public object insider_interview { get; set; }
        public string name { get; set; }
        public bool? premium { get; set; }
        public string published_at { get; set; }
        public List<object> relations { get; set; }
        public bool? response_letter_required { get; set; }
        public object response_url { get; set; }
        public Salary Salary { get; set; }
        public Snippet Snippet { get; set; }
        public object sort_point_distance { get; set; }
        public Type Type { get; set; }
        public string url { get; set; }
    }
}
