using Microsoft.AspNetCore.Mvc;
using Bogus;
using System;
using System.Collections.Generic;

namespace KriptoWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoCurrencyController : ControllerBase
    {
        private static readonly Faker Faker = new Faker();

        [HttpGet]
        [Route("GetCryptoCurrencies")]
        public IEnumerable<CryptoCurrency1> GetCryptoCurrencies()
        {
            var cryptoCurrencies = new List<CryptoCurrency1>
            {
                new CryptoCurrency1 { Symbol = "BTC", Name = "Bitcoin", Price = (decimal)Faker.Finance.Amount(0, 1000), Change24h = (decimal)Faker.Finance.Amount(-10, 10), Date = DateTime.Now },
                new CryptoCurrency1 { Symbol = "ETH", Name = "Ethereum", Price = (decimal)Faker.Finance.Amount(0, 1000), Change24h = (decimal)Faker.Finance.Amount(-10, 10), Date = DateTime.Now },
                new CryptoCurrency1 { Symbol = "BNB", Name = "Binance Coin", Price = (decimal)Faker.Finance.Amount(0, 1000), Change24h = (decimal)Faker.Finance.Amount(-10, 10), Date = DateTime.Now },
                new CryptoCurrency1 { Symbol = "DOGE", Name = "Dogecoin", Price = (decimal)Faker.Finance.Amount(0, 1000), Change24h = (decimal)Faker.Finance.Amount(-10, 10), Date = DateTime.Now }
            };

            return cryptoCurrencies;
        }
    }

    public class CryptoCurrency1
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Change24h { get; set; }
        public DateTime Date { get; set; }
    }
}
