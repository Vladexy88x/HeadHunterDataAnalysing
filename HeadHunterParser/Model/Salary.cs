using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Model
{
    class Salary
    {
        private string currency { get; set; }
        public int? from { get; set; }
        private bool? gross { get; set; }
        public object to { get; set; }
    }
}
