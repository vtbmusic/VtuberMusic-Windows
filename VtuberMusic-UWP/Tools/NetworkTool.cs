using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace VtuberMusic_UWP.Tools
{
    class NetworkTool
    {
        public static async Task<string> PostApiAsync(string uri, object content, bool needAuth = false)
        {
            HttpClient client = new HttpClient();
            if (needAuth)
            {
                client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("");
            }

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                var response = await client.PostAsync(new Uri(uri),
                    new HttpStringContent(JsonConvert.SerializeObject(content, jsonSerializerSettings),
                    UnicodeEncoding.Utf8, "application/json"));

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<T> PostApiAsync<T>(string uri, object content, bool needAuth = false)
        {
            HttpClient client = new HttpClient();
            if (needAuth)
            {
                client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("");
            }

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                IHttpContent httpContent;
                if (content.GetType() != typeof(string))
                {
                    httpContent = new HttpStringContent(JsonConvert.SerializeObject(content, jsonSerializerSettings));
                }
                else
                {
                    httpContent = new HttpStringContent(content.ToString());
                }

                httpContent.Headers.ContentType = new HttpMediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(new Uri(uri), httpContent);

                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
