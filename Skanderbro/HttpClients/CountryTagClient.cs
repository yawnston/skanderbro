using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Polly;
using Serilog;

namespace Skanderbro.HttpClients
{
    public sealed class CountryTagClient : ICountryTagClient
    {
        private const string CountryTagResourceUrl = "https://eu4cheats.com/country-tags";

        private static readonly HttpStatusCode[] RepeatableStatusCodes = new HttpStatusCode[] {
           HttpStatusCode.RequestTimeout, // 408
           HttpStatusCode.InternalServerError, // 500
           HttpStatusCode.BadGateway, // 502
           HttpStatusCode.ServiceUnavailable, // 503
           HttpStatusCode.GatewayTimeout // 504
        };

        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public CountryTagClient(HttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<IReadOnlyDictionary<string, string>> GetCountryTagsAsync()
        {
            HttpResponseMessage response;
            try
            {
                response = await Policy
                        .Handle<HttpRequestException>()
                        .OrResult<HttpResponseMessage>(r => RepeatableStatusCodes.Contains(r.StatusCode))
                        .WaitAndRetryAsync(new List<TimeSpan> { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                        .ExecuteAsync(() => httpClient.GetAsync(CountryTagResourceUrl));
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is SocketException)
                {
                    logger.Error(ex, "Could not fetch country tag list");
                    return new Dictionary<string, string>();
                }
                throw;
            }

            if (!response.IsSuccessStatusCode)
            {
                logger.Error($"Could not fetch country tag list - status code {response.StatusCode}");
                return new Dictionary<string, string>();
            }

            var page = await response.Content.ReadAsStringAsync();
            return CreateCountryDictionaryFromHtml(page);
        }

        private static IReadOnlyDictionary<string, string> CreateCountryDictionaryFromHtml(string page)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(page);
            var nodes = doc.GetElementbyId("table").SelectNodes("tbody/tr/td");

            var dict = new Dictionary<string, string>();
            string previousCountryName = null;
            foreach (var node in nodes)
            {
                if (previousCountryName == null)
                {
                    previousCountryName = node.InnerText;
                }
                else
                {
                    dict.Add(previousCountryName, node.InnerText);
                    previousCountryName = null;
                }
            }

            return dict;
        }
    }
}
