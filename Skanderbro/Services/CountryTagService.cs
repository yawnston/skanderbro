using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fastenshtein;
using Microsoft.Extensions.Caching.Memory;
using Skanderbro.HttpClients;
using Skanderbro.Models;

namespace Skanderbro.Services
{
    public sealed class CountryTagService : ICountryTagService
    {
        private const string CountryTagsMemoryCacheKey = "_CountryTags";

        private readonly ICountryTagClient countryTagClient;
        private readonly IMemoryCache memoryCache;

        public CountryTagService(ICountryTagClient countryTagClient, IMemoryCache memoryCache)
        {
            this.countryTagClient = countryTagClient;
            this.memoryCache = memoryCache;
        }

        public async Task<CountryTagResult> GetCountryTag(string countryName)
        {
            if (!memoryCache.TryGetValue(CountryTagsMemoryCacheKey, out IReadOnlyDictionary<string, string> countryNamesAndTags))
            {
                countryNamesAndTags = await countryTagClient.GetCountryTagsAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(12));

                memoryCache.Set(CountryTagsMemoryCacheKey, countryNamesAndTags, cacheEntryOptions);
            }

            return FindCountryTag(countryName, countryNamesAndTags);
        }

        private static CountryTagResult FindCountryTag(string countryName, IReadOnlyDictionary<string, string> countryNamesAndTags)
        {
            int minimumDistanceFound = int.MaxValue;
            (string, string) bestCountryFound = (null, null);
            foreach (var nameAndTag in countryNamesAndTags)
            {
                int distance = Levenshtein.Distance(countryName.ToUpperInvariant(), nameAndTag.Key.ToUpperInvariant());
                if (distance == 0)
                {
                    return new CountryTagResult
                    {
                        LevenshteinDistance = 0,
                        Name = nameAndTag.Key,
                        Tag = nameAndTag.Value
                    };
                }
                if (distance < minimumDistanceFound)
                {
                    minimumDistanceFound = distance;
                    bestCountryFound = (nameAndTag.Key, nameAndTag.Value);
                }
            }

            if (bestCountryFound.Item1 != null && bestCountryFound.Item2 != null)
            {
                return new CountryTagResult
                {
                    LevenshteinDistance = minimumDistanceFound,
                    Name = bestCountryFound.Item1,
                    Tag = bestCountryFound.Item2
                };
            }
            return null;
        }
    }
}
