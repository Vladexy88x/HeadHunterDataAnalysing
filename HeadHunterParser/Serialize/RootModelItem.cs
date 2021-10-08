using HeadHunterParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Serialize
{
    class RootModelItem
    {
        public string name { get; set; }
        public int? froms { get; set; }
        public object to { get; set; }
        public string currency { get; set; }
        public string nalog { get; set; }
        public string finalSalary { get; set; }
        public string experienceInfo { get; set; }
    }
}
