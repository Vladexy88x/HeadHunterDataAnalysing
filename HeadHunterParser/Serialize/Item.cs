using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HeadHunterParser.Serialize
{
    class Item
    {
         public string id { get; set; }
         public string text { get; set; }
         public string url { get; set; }
    }
}
