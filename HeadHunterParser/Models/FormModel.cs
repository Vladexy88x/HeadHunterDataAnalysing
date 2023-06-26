using System.Windows.Controls;

namespace HeadHunterParser.Models
{
    public class FormModel
    {
        public int Id { get; set; }
        public string TextInput { get; set; }
        public DataGrid DataGrid { get; set; }
        public CheckBox StayImportantInfoCheck { get; set; }
        public CheckBox ExperienceCheck { get; set; }
        public RadioButton RadioButtonBetweenLow { get; set; }
        public RadioButton RadioButtonNoExperience { get; set; }
        public RadioButton RadioButtonBetweenMiddle { get; set; }
        public RadioButton RadioButtonBetweenHigh { get; set; }
    }
}
