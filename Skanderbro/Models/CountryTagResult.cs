namespace Skanderbro.Models
{
    public sealed class CountryTagResult
    {
        public string Name { get; set; }
        public int LevenshteinDistance { get; set; }
        public string Tag { get; set; }
    }
}
