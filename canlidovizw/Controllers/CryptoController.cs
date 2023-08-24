
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Canlidovizw.Controllers
{
    public class CryptoController : Controller
    {
        private readonly IHubContext<CryptoHub> _cryptoHubContext;

        public CryptoController(IHubContext<CryptoHub> cryptoHubContext)
        {
            _cryptoHubContext = cryptoHubContext;
        }

        public async Task<IActionResult> Index()
        {
            // Verileri SignalR hub'ına gönder
            await _cryptoHubContext.Clients.All.SendAsync("GetCryptoCurrencies");

            return View();
        }
    }
}
