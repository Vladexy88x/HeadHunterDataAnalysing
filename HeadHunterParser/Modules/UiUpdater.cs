using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HeadHunterParser.Modules
{
    public class UiUpdater
    {
        public void UpdateListBox(Dictionary<string, string> pairs, ListBox listBox)
        {
            foreach (var item in pairs)
            {
                //_areaCollectionId.Add(item.Key);
                listBox.Items.Add(item.Value);
            }
        }
    }
}
