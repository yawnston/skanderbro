using System.Threading.Tasks;
using Skanderbro.Models;

namespace Skanderbro.Services
{
    public interface ICountryTagService
    {
        Task<CountryTagResult> GetCountryTag(string countryName);
    }
}
