using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Model
{
    class Contacts
    {
        public string email { get; set; }
        public string name { get; set; }
        public List<object> phones { get; set; }
    }
}
