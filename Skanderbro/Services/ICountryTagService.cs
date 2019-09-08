using System.Threading.Tasks;

namespace Skanderbro.Services
{
    public interface ICountryTagService
    {
        Task<string> GetCountryTag(string countryName);
    }
}