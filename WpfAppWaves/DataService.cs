using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WpfAppWaves
{
	public class DataService
	{  
		public static async Task<string> SendRequest(string fullUrl)
		{
			var response = string.Empty;
			using (var client = new HttpClient())
			{
				HttpResponseMessage result = await client.GetAsync(fullUrl);
				if (result.IsSuccessStatusCode)
				{
					response = await result.Content.ReadAsStringAsync();
				}
			}
			return response;
		}

        public static async Task<Root> GetMarketData(string url, string query)
        {
            string fullUrl = url + query;
            Root marketData = new Root();

            string response = await SendRequest(fullUrl);

            try
            {
                marketData = JsonConvert.DeserializeObject<Root>(response);
            }
			catch {  }

            return marketData;           
        }

        public static string ReadFilePath()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            string path = System.Environment.CurrentDirectory;
            string fileName = "TestData.txt";

            if (dlg.ShowDialog() == true)
            {
                path = dlg.FileName;
                fileName = dlg.SafeFileName;
            }
            else
            {
                throw (new Exception("Select file TestData.txt"));
            }

            return path;
        }

        public static void SaveFile(string text)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = ".json";
            sfd.AddExtension = true;
            sfd.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";

            if (sfd.ShowDialog() == true)
                File.WriteAllText(sfd.FileName, text);
        }
    }
}
