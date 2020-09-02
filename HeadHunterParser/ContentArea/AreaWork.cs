using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HeadHunterParser.Model;
using HeadHunterParser.SearchArea;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace HeadHunterParser.ContentArea
{
    class AreaWork
    {
        private List<string> AreaCollectionId { get; set; }

        private int id { get; set; }
        private DataGrid dataGrid { get; set; }
        private string textInput { get; set; }
        private CheckBox stayImportantInfoCheck { get; set; }
        private RadioButton radioButtonNoExperience { get; set; }
        private RadioButton radioButtonBetweenLow { get; set; }
        private RadioButton radioButtonBetweenMiddle { get; set; }
        private RadioButton radioButtonBetweenHigh { get; set; }
        private CheckBox experienceCheck { get; set; }

        public AreaWork(int id, DataGrid dataGrid, string textInput, CheckBox stayImportantInfoCheck,
            CheckBox experienceCheck, RadioButton radioButtonBetweenLow, RadioButton radioButtonNoExperience,
            RadioButton radioButtonBetweenMiddle, RadioButton radioButtonBetweenHigh)
        {
            AreaCollectionId = new List<string>();
            this.id = id;
            this.dataGrid = dataGrid;
            this.textInput = textInput;
            this.stayImportantInfoCheck = stayImportantInfoCheck;
            this.radioButtonNoExperience = radioButtonNoExperience;
            this.experienceCheck = experienceCheck;
            this.radioButtonBetweenLow = radioButtonBetweenLow;
            this.radioButtonBetweenMiddle = radioButtonBetweenMiddle;
            this.radioButtonBetweenHigh = radioButtonBetweenHigh;
        }
        public async void GetAreas(string currentArea, ListBox listBox)
        {
            string content = "";
            string url = $"https://api.hh.ru/suggests/areas?text={currentArea}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36";
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = await reader.ReadToEndAsync();
                    reader.Close();
                }
                response.Close();
            }

            Dictionary<string, string> pairs = new Dictionary<string, string>();
            RootItemObject area = JsonConvert.DeserializeObject<RootItemObject>(content);
            for (var i = 0; i < area.items.Count; i++)
            {
                pairs.Add(area.items[i].id, area.items[i].text);
            }
            foreach (var item in pairs)
            {
                AreaCollectionId.Add(item.Key);
                listBox.Items.Add(item.Value);
            }
        }

        public async void GetInfoArea(int id, DataGrid dataGrid)
        {
            var areaId = AreaCollectionId[id];
            string content = "";
            var url = $"https://api.hh.ru/vacancies?area={areaId}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36";
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = await reader.ReadToEndAsync();
                    reader.Close();
                }
                response.Close();
            }

            var area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            for (var i = 0; i < area.Items.Count; i++)
            {
                collectionInfo.Add(area.Items[i].Snippet.requirement + " - " + area.Items[i].name);
            }
            dataGrid.ItemsSource = area.Items;
        }

        public async void GetInfoAreaWitchSearch()
        {
            var areaId = AreaCollectionId[id];
            string content = "";
            var url = $"https://api.hh.ru/vacancies?area={areaId}&text={textInput}";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36";
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    content = await reader.ReadToEndAsync();
                    reader.Close();
                }
                response.Close();
            }

            var area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            for (var i = 0; i < area.Items.Count; i++)
            {
                collectionInfo.Add(area.Items[i].Snippet.requirement + " - " + area.Items[i].name);
            }
            dataGrid.ItemsSource = area.Items;
        }

       

        public async void GetInfoAreaWitchSearchNewVerison()
        {
            var areaId = AreaCollectionId[id];
            string content = "";
            var url = $"https://api.hh.ru/vacancies?area={areaId}&text={textInput}";
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36");
            System.Net.Http.HttpResponseMessage response;

            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                response = await httpClient.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            var area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            List<Employer> employers = new List<Employer>();
            List<Salary> salarys = new List<Salary>();
            List<RootModelItem> rootModelItem = new List<RootModelItem>();
            int calculateSalaryWithProcent = 0;
            int calulateSalaryFinal = 0;
            for (var i = 0; i < area.Items.Count; i++)
            {
                if(area.Items[i].Salary?.from == null)
                {
                    continue;
                }
                collectionInfo.Add(area.Items[i].Employer?.name + " : " + area.Items[i].Salary?.from + " - " + area.Items[i].Salary?.to + " руб");
                employers.Add(area.Items[i].Employer);
                salarys.Add(area.Items[i].Salary);
                calculateSalaryWithProcent = (int)area.Items[i].Salary?.from / 100 * 13;
                calulateSalaryFinal = (int)area.Items[i].Salary?.from - calculateSalaryWithProcent;
                rootModelItem.Add(
                new RootModelItem()
                {
                    name = area.Items[i].Employer?.name,
                    froms = area.Items[i].Salary?.from,
                    to = area.Items[i].Salary?.to,
                    currency = "RUB",
                    nalog = "13%",
                    finalSalary = calulateSalaryFinal.ToString("00,000")
                });

            }
            //from / 100 * 13
            if (stayImportantInfoCheck.IsChecked == true)
            {
                dataGrid.ItemsSource = rootModelItem;
            }

          
        }

        public async void GetInfoAreaWithExperience()
        {
            //experience
            var areaId = AreaCollectionId[id];
            string url = "";
            string selectedWorkExperience = "";
            if (experienceCheck.IsChecked == true)
            {
                if (radioButtonNoExperience.IsChecked == true)
                {
                    url = $"https://api.hh.ru/vacancies?area={areaId}&text={textInput}&experience=noExperience";
                    selectedWorkExperience = "Без опыта";
                }
                if (radioButtonBetweenLow.IsChecked == true)
                {
                    url = $"https://api.hh.ru/vacancies?area={areaId}&text={textInput}&experience=between1And3";
                    selectedWorkExperience = "От 1 до 3";
                }
                if (radioButtonBetweenMiddle.IsChecked == true)
                {
                    url = $"https://api.hh.ru/vacancies?area={areaId}&text={textInput}&experience=between3And6";
                    selectedWorkExperience = "От 3 до 6";
                }
                if (radioButtonBetweenHigh.IsChecked == true)
                {
                    url = $"https://api.hh.ru/vacancies?area={areaId}&text={textInput}&experience=moreThan6";
                    selectedWorkExperience = "Более 6 лет";
                }

            }
            else
            {
                return;
            }
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.142 Safari/537.36");
            System.Net.Http.HttpResponseMessage response;
            string content = "";
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                response = await httpClient.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            var area = JsonConvert.DeserializeObject<RootObjectInfoArea>(content);
            var collectionInfo = new List<string>();
            List<Employer> employers = new List<Employer>();
            List<Salary> salarys = new List<Salary>();
            List<RootModelItem> rootModelItem = new List<RootModelItem>();
            int calculateSalaryWithProcent = 0;
            int calulateSalaryFinal = 0;
            for (var i = 0; i < area.Items.Count; i++)
            {
                if (area.Items[i].Salary?.from == null)
                {
                    continue;
                }
                collectionInfo.Add(area.Items[i].Employer?.name + " : " + area.Items[i].Salary?.from + " - " + area.Items[i].Salary?.to + " руб");
                employers.Add(area.Items[i].Employer);
                salarys.Add(area.Items[i].Salary);
                calculateSalaryWithProcent = (int)area.Items[i].Salary?.from / 100 * 13;
                calulateSalaryFinal = (int)area.Items[i].Salary?.from - calculateSalaryWithProcent;
                rootModelItem.Add(
                new RootModelItem()
                {
                    name = area.Items[i].Employer?.name,
                    froms = area.Items[i].Salary?.from,
                    to = area.Items[i].Salary?.to,
                    currency = "RUB",
                    nalog = "13%",
                    finalSalary = calulateSalaryFinal.ToString("00,000"),
                    experienceInfo = selectedWorkExperience
                });

            }
            if (experienceCheck.IsChecked == true)
            {
                dataGrid.ItemsSource = rootModelItem;
            }
        }

        
    }
}
