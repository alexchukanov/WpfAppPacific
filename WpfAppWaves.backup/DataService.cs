using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
			catch { }

            return marketData;           
        }

        /*
        private static async Task<string> SendRequest(string fullUrl)
        {
            string response = "";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Uri connectionUri = new Uri(fullUrl, UriKind.RelativeOrAbsolute);

                try
                {
                    response = await client.GetStringAsync(connectionUri);
                }
                catch (Exception ex)
                {
                    client.CancelPendingRequests();
                    return "Connection error!";
                }
                finally
                {
                    client.CancelPendingRequests();
                }
            }

            return response;
        } 
        */
    }
}
