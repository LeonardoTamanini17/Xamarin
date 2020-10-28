using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace projeto_login.Services
{
    public class DataService
    {
        public static async Task<JToken> GetToken()
        {
            var Url = "https://run.mocky.io/v3/83599a37-9b03-47d1-970d-555f8835355c";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(Url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string resultContent = response.Content.ReadAsStringAsync().Result;

                    if (resultContent != "")
                    {
                        var token = JToken.Parse(resultContent);
                        return token;
                    }
                    return null;
                }
            }
            return null;
        }
    }
}
