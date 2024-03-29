﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HeadHunterParser.Modules
{
    public class HttpService
    {
        private string _userAgentValue;

        public HttpService(string userAgentValue)
        {
            _userAgentValue = userAgentValue;
        }

        public async Task<string> GetAsync(string url)
        {
            var content = "";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = _userAgentValue;
            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        content = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return content;
        }
    }
}
