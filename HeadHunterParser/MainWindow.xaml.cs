using HeadHunterParser.General;
using HeadHunterParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace HeadHunterParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _selectArea;
        private readonly AreaWork _areaWork;

        public MainWindow()
        {
            InitializeComponent();
            var formModel = new FormModel
            {
                Id = _selectArea,
                TextInput = SearchInput.Text,
                DataGrid = dataGrid,
                StayImportantInfoCheck = stayImportantInfoCheck,
                ExperienceCheck = experienceCheck,
                RadioButtonBetweenLow = radioBtnWithBetweenLow,
                RadioButtonNoExperience = radioBtnNoExperience,
                RadioButtonBetweenMiddle = radioBtnNoExperience,
                RadioButtonBetweenHigh = radioBtnBetweenHigh
            };
            _areaWork = new AreaWork(formModel);
        }

        private void SelectAreaButton_Click(object sender, RoutedEventArgs e)
        {
            listboxFirst.Items.Clear();
            _areaWork.GetAsync(inputArea.Text, listBox: listboxFirst);
        }

        private void ListboxFirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectArea = listboxFirst.SelectedIndex;
            areaLbl.Content = $"Регион {listboxFirst.SelectedItem}";
        }

        private async void StartWorkButton_Click(object sender, RoutedEventArgs e)
        {
            int taxNumber;
            bool isValidateNalog = int.TryParse(taxTexb.Text, out taxNumber);
            if (!isValidateNalog && taxNumber < 100 && !string.IsNullOrEmpty(taxTexb.Text))
            {
                MessageBox.Show("Уберите символы из строки Налог, и оставьте только число не больше 100");
            }
            if (string.IsNullOrWhiteSpace(inputArea.Text))
            {
                MessageBox.Show("Область не выбрана");
                return;
            }
            await _areaWork.GetInfoAsync(_selectArea, dataGrid, taxNumber);
            await _areaWork.GetInfoWithExperienceAsync(taxNumber);
        }

        private void TestAddColumnBtn_Click(object sender, RoutedEventArgs e)
        {
            _areaWork.Dispose();
        }

        private void FirstCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void FirstCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ClearGettingInfoButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
        }
    }
}
