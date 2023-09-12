using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace tcc_mypet_back.Services
{
    public class GeocodingHelper
    {
        private readonly string _apiKey;

        public GeocodingHelper(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public async Task<(double Latitude, double Longitude)> GetLatLongFromAddress(string address)
        {
            string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);

                // Pegar a latitude e longitude
                var location = json["results"][0]["geometry"]["location"];
                double lat = location["lat"].Value<double>();
                double lng = location["lng"].Value<double>();

                return (lat, lng);
            }
        }
    }
}