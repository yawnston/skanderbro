using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skanderbro.HttpClients
{
    public interface ICountryTagClient
    {
        Task<IReadOnlyDictionary<string, string>> GetCountryTagsAsync();
    }
}
