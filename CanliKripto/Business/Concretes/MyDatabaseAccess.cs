using CanliKripto.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CanliKripto.Business.Concretes
{
    public class MyDatabaseAccess
    {
        public List<CryptoData> GetLatestCryptoData()
        {
            // Veritabanı bağlantı dizesini ve gerekli Entity Framework Core ayarlarını yapın
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CryptoDB;Trusted_Connection=True;";
            var options = new DbContextOptionsBuilder<CryptoCurrencyDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Veritabanı bağlamını oluşturun
            using (var dbContext = new CryptoCurrencyDbContext(options))
            {
                //CryptoCurrencies tablosundan en son veriyi almak için OrderByDescending ile
                //Id alanına göre sıralama yapılıyor ve Take(1) ile en son kaydı alınıyor.
                //Select ile CryptoData nesnesi oluşturuluyor ve latestCryptoCurrencies listesine ekleniyor.
                // CryptoCurrency tablosundaki son verileri alıyor
                var latestCryptoCurrencies = dbContext.CryptoCurrencies
                    .OrderByDescending(c => c.Id)
                    .Take(1)
                    .Select(c => new CryptoData
                    {
                        Symbol = c.Symbol,
                        Name = c.Name
                    })
                    .ToList();

                // CryptoCurrencyValue tablosundaki son verileri alın
                var latestCryptoCurrencyValues = dbContext.CryptoCurrencyValues
                    .OrderByDescending(v => v.Id)
                    .Take(1)
                    .Select(v => new CryptoData
                    {
                        Price = v.Price,
                        Change24h = v.Change24h,
                        Date = v.Date
                    })
                    .ToList();

                // CryptoCurrency ve CryptoCurrencyValue verilerini birleştirin
                var mergedData = latestCryptoCurrencies
                    .Join(latestCryptoCurrencyValues,
                        c => 1, // Dummy key
                        v => 1, // Dummy key
                        (c, v) => new CryptoData
                        {
                            Symbol = c.Symbol,
                            Name = c.Name,
                            Price = v.Price,
                            Change24h = v.Change24h,
                            Date = v.Date
                        })
                    .ToList();

                return mergedData;
            }
        }
    }

    public class CryptoData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public decimal? Change24h { get; set; }
        public DateTime? Date { get; set; }
    }

}