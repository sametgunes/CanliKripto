using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Canlidovizw.Controllers
{
    public class CryptoHub : Hub
    {
        public async Task<dynamic[]> GetCryptoCurrencies()
        {
            using (var client = new HttpClient())
            {
                string apiUrl = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=bitcoin,ethereum,binancecoin,dogecoin";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var cryptoCurrencies = JsonSerializer.Deserialize<dynamic[]>(responseBody);

                return cryptoCurrencies;
            }
        }
    }

}
