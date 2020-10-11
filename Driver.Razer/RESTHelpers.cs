using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Driver.Razer
{
    public static class RESTHelpers
    {
        public static async Task<T> PostAsync<T>(string url, string model)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(model, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(result);
            }
        }


        public static void Put<T>(string url, T model)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync(url, data).Result;
            }
        }

        public static void Put(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.PutAsync(url, null).Result;
            }
        }


        public static void PutString(string url, string json)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync(url, data).Result;
            }
        }

        public static void Delete(string url)
        {
            using (var client = new HttpClient())
            {
                client.DeleteAsync(url);
            }
        }
    }
}
