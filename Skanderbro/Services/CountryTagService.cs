using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System;

namespace Skanderbro.Services
{
    public sealed class CountryTagService : ICountryTagService
    {
        private readonly HttpClient httpClient;

        public CountryTagService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> GetCountryTag(string countryName)
        {
            var response = await httpClient.GetAsync("https://eu4cheats.com/country-tags");
            var page = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(page);

            var nodes = doc.GetElementbyId("table").SelectNodes("tbody/tr/td");

            bool foundMatch = false;
            foreach (var node in nodes)
            {
                if (foundMatch)
                {
                    return node.InnerText;
                }
                if (string.Equals(node.InnerText, countryName, StringComparison.InvariantCultureIgnoreCase))
                {
                    foundMatch = true;
                }
            }

            return null;
        }
    }
}
