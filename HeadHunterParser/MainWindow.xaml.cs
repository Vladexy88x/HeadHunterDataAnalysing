﻿using HeadHunterParser.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
            _areaWork = new AreaWork(_selectArea,
                                     SearchInput.Text,
                                     dataGrid,
                                     stayImportantInfoCheck,
                                     experienceCheck,
                                     radioBtnWithBetweenLow,
                                     radioBtnNoExperience,
                                     radioBtnBetweenMiddle,
                                     radioBtnBetweenHigh);
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

        private void StartWorkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchInput.Text))
            {
                _areaWork.GetInfoAsync(_selectArea, dataGrid);
                _areaWork.GetInfoWithExperienceAsync();
            }
            else
            {
                _areaWork.GetInfoWithExperienceAsync();
                _areaWork.Dispose();
            }
        }

        private void TestAddColumnBtn_Click(object sender, RoutedEventArgs e)
        {
          
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
