using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HeadHunterParser.Models;
using HeadHunterParser.Modules;
using HeadHunterParser.Serialize;

namespace HeadHunterParser.General
{
    public class AreaWork : IDisposable
    {
        private int _id;
        private string _textInput;
        private List<string> _areaCollectionId;
        private DataGrid _dataGrid;
        private CheckBox _stayImportantInfoCheck;
        private RadioButton _radioButtonNoExperience;
        private RadioButton _radioButtonBetweenLow;
        private RadioButton _radioButtonBetweenMiddle;
        private RadioButton _radioButtonBetweenHigh;
        private CheckBox _experienceCheck;
        private const string _userAgentValue = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36";
        private const string _currency = "RUB";

        private readonly HttpService _httpService;
        private readonly JsonParser _jsonParser;
        private readonly UiUpdater _uiUpdater;

        public AreaWork(FormModel formModel)
        {
            _areaCollectionId = new List<string>();
            _id = formModel.Id;
            _dataGrid = formModel.DataGrid;
            _textInput = formModel.TextInput;
            _stayImportantInfoCheck = formModel.StayImportantInfoCheck;
            _radioButtonNoExperience = formModel.RadioButtonNoExperience;
            _experienceCheck = formModel.ExperienceCheck;
            _radioButtonBetweenLow = formModel.RadioButtonBetweenLow;
            _radioButtonBetweenMiddle = formModel.RadioButtonBetweenMiddle;
            _radioButtonBetweenHigh = formModel.RadioButtonBetweenHigh;

            _httpService = new HttpService(_userAgentValue);
            _jsonParser = new JsonParser();
            _uiUpdater = new UiUpdater();
        }

        public async void GetAsync(string currentArea, ListBox listBox)
        {
            string url = $"https://api.hh.ru/suggests/areas?text={currentArea}";
            string content = await _httpService.GetAsync(url);
            var pairs = _jsonParser.ParseResponse(content);
            foreach (var item in pairs)
            {
                _areaCollectionId.Add(item.Key);
            }
            _uiUpdater.UpdateListBox(pairs, listBox);
        }

        public async Task GetInfoAsync(int id, DataGrid dataGrid, int tax)
        {
            if (_areaCollectionId.Count <= 0)
                return;

            if(_experienceCheck.IsChecked == true)
            {
                await GetDetailsWithAdditionalAsync(id, dataGrid, tax);
                return;
            }

            string areaId = _areaCollectionId[id];
            string url = $"https://api.hh.ru/vacancies?area={areaId}";
            string content = await _httpService.GetAsync(url);
            RootObjectInfoArea rootArea = _jsonParser.ParseResponseRootInfoArea(content);

            dataGrid.ItemsSource = rootArea.Items;
        }

        public async Task GetDetailsWithAdditionalAsync(int id, DataGrid dataGrid, int tax)
        {
            if (_areaCollectionId.Count <= 0)
                return;

            string areaId = _areaCollectionId[id];
            string url = $"https://api.hh.ru/vacancies?area={areaId}";
            string content = await _httpService.GetAsync(url);
            RootObjectInfoArea rootArea = _jsonParser.ParseResponseRootInfoArea(content);

            dataGrid.ItemsSource = AdditionalDetails(rootArea, tax);
        }

        public async void GetInfoWitchSearchAsync()
        {
            if (_areaCollectionId.Count <= 0)
                return;

            string areaId = _areaCollectionId[_id];
            string url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}";
            string content = await _httpService.GetAsync(url);
            RootObjectInfoArea rootArea = _jsonParser.ParseResponseRootInfoArea(content);

            _dataGrid.ItemsSource = rootArea.Items;
        }

        private string GetVacancyUrl()
        {
            if (_areaCollectionId.Count <= 0)
                return "";

            string areaId = _areaCollectionId[_id];

            if (_radioButtonNoExperience.IsChecked == true)
            {
                return $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=noExperience";
            }
            if (_radioButtonBetweenLow.IsChecked == true)
            {
                return $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=between1And3";
            }
            if (_radioButtonBetweenMiddle.IsChecked == true)
            {
                return $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=between3And6";
            }
            if (_radioButtonBetweenHigh.IsChecked == true)
            {
                return $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=moreThan6";
            }
            return "";
        }
        
        private int CalculateTax(int tax, int salary)
        {
            return salary / 100 * tax;
        }

        private int SalaryСalculationWithTax(int salary, int procent)
        {
            return salary - procent;
        }

        private List<RootModelItem> AdditionalDetails(RootObjectInfoArea rootArea, int tax = 0, string selectedWorkExperience = "")
        {
            var rootModelItem = new List<RootModelItem>();
            for (var i = 0; i < rootArea.Items.Count; i++)
            {
                if (rootArea.Items[i].Salary?.from == null ||
                    rootArea.Items[i].Employer?.name == null ||
                    rootArea.Items[i].Salary?.to == null)
                    continue;

                var salary = (int)rootArea.Items[i].Salary?.from;
                var calculateSalaryWithProcent = CalculateTax(tax, salary);
                var calulateSalaryFinal = SalaryСalculationWithTax(salary, calculateSalaryWithProcent);

                rootModelItem.Add(
                new RootModelItem()
                {
                    name = rootArea.Items[i].Employer?.name,
                    froms = rootArea.Items[i].Salary?.from,
                    to = rootArea.Items[i].Salary?.to,
                    currency = _currency,
                    nalog = $"{tax}%",
                    finalSalary = calulateSalaryFinal.ToString("00,000"),
                    experienceInfo = selectedWorkExperience
                });
            }
            return rootModelItem;
        }
        public async Task GetInfoWithExperienceAsync(int tax)
        {
            var url = GetVacancyUrl();
            var selectedWorkExperience = GetVacancyUrl();

            if (_experienceCheck.IsChecked == false || selectedWorkExperience == "")
                return;

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Опыт работы не выбран");
                return;
            }

            string content = await _httpService.GetAsync(url);
            RootObjectInfoArea rootArea = _jsonParser.ParseResponseRootInfoArea(content);

            if (rootArea == null)
            {
                throw new Exception("Area null");
            }

           
            _dataGrid.ItemsSource = AdditionalDetails(rootArea, tax, selectedWorkExperience);
        }

        public void Dispose()
        {
            _areaCollectionId.Clear();
        }
    }
}