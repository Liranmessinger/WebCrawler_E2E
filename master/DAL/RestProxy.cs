using master.Contracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace master.DAL
{
    public class RestProxy
    {
        private readonly string restServiceURL;
        private readonly Uri baseUri;


        public RestProxy(string restServiceURL)
        {
            this.restServiceURL = restServiceURL;
            Uri.TryCreate(this.restServiceURL, UriKind.Absolute, out baseUri);
        }

        public async Task<WebCrawlerNode> GetRestCallAsyncResponse(string uriRequest)
        {
            try
            {
                WebCrawlerNode result=new WebCrawlerNode();
                using (var client = new HttpClient())
                {
                    var jsonRequest=new { url = uriRequest };
                    var stringContent = new StringContent( JsonSerializer.Serialize(jsonRequest), Encoding.UTF8, "application/json");
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = this.baseUri,
                        Content = stringContent
                    };

                    HttpResponseMessage response = await client.SendAsync(request);
                    using (HttpContent content = response.Content)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        result = JsonSerializer.Deserialize<WebCrawlerNode>(jsonString);
                    }
                }
                return result;
            }
            catch (HttpRequestException ex) // Non success
            {
            Log.Error($"error at GetJsonHttpClient {ex}");

            }
            catch (NotSupportedException ex) // When content type is not valid
            {
                Log.Error($"error at GetJsonHttpClient {ex}");
            }
            catch (JsonException ex) // Invalid JSON
            {
                Log.Error($"error at GetJsonHttpClient {ex}");

            }

            return null;
        }
    }
}
