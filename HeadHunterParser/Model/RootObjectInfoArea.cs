using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Model
{
    class RootObjectInfoArea
    {
        public List<Item> Items { get; set; }
        public string alternate_url { get; set; }
        public object arguments { get; set; }
        public object clusters { get; set; }
        public int found { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
        public int per_page { get; set; }
    }
}
