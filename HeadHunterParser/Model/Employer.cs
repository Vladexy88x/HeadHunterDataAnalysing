namespace HeadHunterParser.Model
{
    class Employer
    {
        private string alternate_url { get; set; }
        private string id { get; set; }
        public string name { get; set; }
        private bool? trusted { get; set; }
        private string url { get; set; }
        private string vacancies_url { get; set; }
    }
}
