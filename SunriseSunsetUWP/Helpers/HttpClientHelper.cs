using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SunriseSunsetUWP.Helpers
{
    public class HttpClientHelper
    {
        /// <summary>
        /// Send http request to retrieve Json
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> SendHttpRequest(string url)
        {
            var uri = new Uri(url);
            var httpClient = new HttpClient();

            // Always catch network exceptions for async methods
            try
            {
                var result = await httpClient.GetStringAsync(uri);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // Details in ex.Message and ex.HResult.    
                return string.Empty;
            }
        }
    }
}
