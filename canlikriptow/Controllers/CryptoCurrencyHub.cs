using KriptoWebApiProject.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CanliKripto.Hubs
{
    public class CryptoCurrencyHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }


        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task SendCryptoCurrencyData(List<CryptoCurrency1> cryptoCurrencies)
        {
            await Clients.All.SendAsync("ReceiveCryptoCurrencyData", cryptoCurrencies);
        }
        public async Task<List<CryptoCurrency1>> GetCryptoCurrencies()
        {
            var cryptoCurrencies = new List<CryptoCurrency1>();

            // Verileri alma kodlarını buraya 

            return cryptoCurrencies;
        }
    }

}