namespace FlowManager.Client.Services
{
    public static class WeatherCache
    {
        private static WeatherInfo? _cachedWeather;
        private static DateTime _cacheTime = DateTime.MinValue;
        private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(30);

        public static WeatherInfo? CachedWeather
        {
            get => _cachedWeather;
            set
            {
                _cachedWeather = value;
                _cacheTime = DateTime.Now;
            }
        }

        public static bool IsExpired()
        {
            return DateTime.Now - _cacheTime > CacheExpiry;
        }

        public static void ClearCache()
        {
            _cachedWeather = null;
            _cacheTime = DateTime.MinValue;
        }
    }

    public class WeatherInfo
    {
        public string? Description { get; set; }
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public string? Main { get; set; }
        public string? Icon { get; set; }
        public double WindSpeed { get; set; }
        public int Visibility { get; set; }
        public int Pressure { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}
