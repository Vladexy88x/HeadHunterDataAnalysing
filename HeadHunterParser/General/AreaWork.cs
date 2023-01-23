using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using HeadHunterParser.Model;
using HeadHunterParser.Serialize;
using Newtonsoft.Json;

namespace HeadHunterParser.General
{
    public class AreaWork : IDisposable
    {
        private int _id;
        private readonly List<string> _areaCollectionId;
        private string _textInput;
        private DataGrid _dataGrid;
        private CheckBox _stayImportantInfoCheck;
        private RadioButton _radioButtonNoExperience;
        private RadioButton _radioButtonBetweenLow;
        private RadioButton _radioButtonBetweenMiddle;
        private RadioButton _radioButtonBetweenHigh;
        private CheckBox _experienceCheck;
        private const string _userAgentName = "User-Agent";
        private const string _userAgentValue = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36";
        private const string _currency = "RUB";

        public AreaWork(int id,
                        string textInput,
                        DataGrid dataGrid,
                        CheckBox stayImportantInfoCheck,
                        CheckBox experienceCheck,
                        RadioButton radioButtonBetweenLow,
                        RadioButton radioButtonNoExperience,
                        RadioButton radioButtonBetweenMiddle,
                        RadioButton radioButtonBetweenHigh)
        {
            _areaCollectionId = new List<string>();
            this._id = id;
            this._dataGrid = dataGrid;
            this._textInput = textInput;
            this._stayImportantInfoCheck = stayImportantInfoCheck;
            this._radioButtonNoExperience = radioButtonNoExperience;
            this._experienceCheck = experienceCheck;
            this._radioButtonBetweenLow = radioButtonBetweenLow;
            this._radioButtonBetweenMiddle = radioButtonBetweenMiddle;
            this._radioButtonBetweenHigh = radioButtonBetweenHigh;
        }
        public async void GetAsync(string currentArea, ListBox listBox)
        {
            var content = "";
            string url = $"https://api.hh.ru/suggests/areas?text={currentArea}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = _userAgentValue;
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = await reader.ReadToEndAsync();
                    reader.Close();
                }
                response.Close();
            }

            var pairs = new Dictionary<string, string>();
            RootItemObject area = JsonConvert.DeserializeObject<RootItemObject>(content);
            if(area.items.Count <= 0)
            {
                MessageBox.Show("Area null");
                return;
            }
            for (var i = 0; i < area.items.Count; i++)
            {
                pairs.Add(area.items[i].id, area.items[i].text);
            }
            foreach (var item in pairs)
            {
                _areaCollectionId.Add(item.Key);
                listBox.Items.Add(item.Value);
            }
        }

        public async void GetInfoAsync(int id, DataGrid dataGrid)
        {
            if(_areaCollectionId.Count <= 0)
                return;

            string areaId = _areaCollectionId[id];
            var content = "";
            string url = $"https://api.hh.ru/vacancies?area={areaId}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = _userAgentValue;
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = await reader.ReadToEndAsync();
                    reader.Close();
                }
                response.Close();
            }

            RootObjectInfoArea area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            for (var i = 0; i < area.Items.Count; i++)
            {
                collectionInfo.Add(area.Items[i].Snippet.requirement + " - " + area.Items[i].name);
            }
            dataGrid.ItemsSource = area.Items;
        }

        public async void GetInfoWitchSearchAsync()
        {
            string areaId = _areaCollectionId[_id];
            var content = "";
            string url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = _userAgentValue;
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = await reader.ReadToEndAsync();
                    reader.Close();
                }
                response.Close();
            }

            RootObjectInfoArea area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            for (var i = 0; i < area.Items.Count; i++)
            {
                collectionInfo.Add(area.Items[i].Snippet.requirement + " - " + area.Items[i].name);
            }
            _dataGrid.ItemsSource = area.Items;
        }

        public async void GetInfoWitchSearchNewVerisonAsync(int tax)
        {
            string areaId = _areaCollectionId[_id];
            var content = "";
            string url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(_userAgentName, _userAgentValue);
            HttpResponseMessage response;

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                response = await httpClient.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            RootObjectInfoArea area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            var employers = new List<Employer>();
            var salarys = new List<Salary>();
            var rootModelItem = new List<RootModelItem>();
            var calculateSalaryWithProcent = 0;
            var calulateSalaryFinal = 0;
            if (area == null)
            {
                MessageBox.Show("Area null");
                return;
            }
            for (var i = 0; i < area.Items.Count; i++)
            {
                if (area.Items[i].Salary?.from == null ||
                    area.Items[i].Employer?.name == null ||
                    area.Items[i].Salary?.to == null)
                    continue;

                collectionInfo.Add(area.Items[i].Employer?.name + " : " + area.Items[i].Salary?.from + " - " + area.Items[i].Salary?.to + " руб");
                employers.Add(area.Items[i].Employer);
                salarys.Add(area.Items[i].Salary);
                calculateSalaryWithProcent = (int)area.Items[i].Salary?.from / 100 * tax;
                calulateSalaryFinal = (int)area.Items[i].Salary?.from - calculateSalaryWithProcent;
                rootModelItem.Add(
                new RootModelItem()
                {
                    name = area.Items[i].Employer?.name,
                    froms = area.Items[i].Salary?.from,
                    to = area.Items[i].Salary?.to,
                    currency = "RUB",
                    nalog = $"{tax}%",
                    finalSalary = calulateSalaryFinal.ToString("00,000")
                });
            }
            //from / 100 * 13
            if (_stayImportantInfoCheck.IsChecked == true)
            {
                _dataGrid.ItemsSource = rootModelItem;
            }
        }
        public async void GetInfoWithExperienceAsync(int tax)
        {
            if (_experienceCheck.IsChecked == false)
                return;

            string areaId = _areaCollectionId[_id];
            var url = "";
            var selectedWorkExperience = "";

            if (_radioButtonNoExperience.IsChecked == true)
            {
                url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=noExperience";
                selectedWorkExperience = "Без опыта";
            }
            if (_radioButtonBetweenLow.IsChecked == true)
            {
                url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=between1And3";
                selectedWorkExperience = "От 1 до 3";
            }
            if (_radioButtonBetweenMiddle.IsChecked == true)
            {
                url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=between3And6";
                selectedWorkExperience = "От 3 до 6";
            }
            if (_radioButtonBetweenHigh.IsChecked == true)
            {
                url = $"https://api.hh.ru/vacancies?area={areaId}&text={_textInput}&experience=moreThan6";
                selectedWorkExperience = "Более 6 лет";
            }
            if(url.Length <= 0)
            {
                MessageBox.Show("Опыт работы не выбран");
                return;
            }
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(_userAgentName, _userAgentValue);
            HttpResponseMessage response;
            var content = "";
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                response = await httpClient.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }

            RootObjectInfoArea area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            var employers = new List<Employer>();
            var salarys = new List<Salary>();
            var rootModelItem = new List<RootModelItem>();
            var calculateSalaryWithProcent = 0;
            var calulateSalaryFinal = 0;
            if(area == null)
            {
                MessageBox.Show("Area null");
                return;
            }
            for (var i = 0; i < area.Items.Count; i++)
            {
                if (area.Items[i].Salary?.from == null || 
                    area.Items[i].Employer?.name == null || 
                    area.Items[i].Salary?.to == null)
                    continue;

                collectionInfo.Add(area.Items[i].Employer?.name + " : " + area.Items[i].Salary?.from + " - " + area.Items[i].Salary?.to + " руб");
                employers.Add(area.Items[i].Employer);
                salarys.Add(area.Items[i].Salary);
                calculateSalaryWithProcent = (int)area.Items[i].Salary?.from / 100 * tax;
                calulateSalaryFinal = (int)area.Items[i].Salary?.from - calculateSalaryWithProcent;
                rootModelItem.Add(
                new RootModelItem()
                {
                    name = area.Items[i].Employer?.name,
                    froms = area.Items[i].Salary?.from,
                    to = area.Items[i].Salary?.to,
                    currency = _currency,
                    nalog = $"{tax}%",
                    finalSalary = calulateSalaryFinal.ToString("00,000"),
                    experienceInfo = selectedWorkExperience
                });
            }
            if (_experienceCheck.IsChecked == true)
            {
                _dataGrid.ItemsSource = rootModelItem;
            }
        }

        public void Dispose()
        {
            _areaCollectionId.Clear();
        }
    }
}
