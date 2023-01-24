using HeadHunterParser.Model;
using HeadHunterParser.Serialize;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Modules
{
    public class JsonParser
    {
        public Dictionary<string, string> ParseResponse(string content)
        {
            var pairs = new Dictionary<string, string>();
            RootItemObject area = JsonConvert.DeserializeObject<RootItemObject>(content);
            if (area.items.Count <= 0)
            {
                throw new Exception("Area not found");
            }
            for (var i = 0; i < area.items.Count; i++)
            {
                pairs.Add(area.items[i].id, area.items[i].text);
            }
            return pairs;
        }

        public RootItemObject ParseResponseRootItem(string content)
        {
            RootItemObject area = JsonConvert.DeserializeObject<RootItemObject>(content);
            if (area.items.Count <= 0)
            {
                throw new Exception("Area not found");
            }
            return area;
        }
        public RootObjectInfoArea ParseResponseRootInfoArea(string content)
        {
            RootObjectInfoArea area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            if (area.Items.Count <= 0)
            {
                throw new Exception("Area not found");
            }
            return area;
        }

        //  RootObjectInfoArea
    }
}
