using CanliKripto.DbContexts;
using CanliKripto.Entities.Concretes;
using KriptoWebApiProject.Controllers;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CanliKripto
{
    public class Program
    {
        private static bool isRunning = true;

        public static async Task Main(string[] args)
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = config.GetConnectionString("DefaultConnection");

                if (connectionString == null)
                {
                    Console.WriteLine("appsettings.json dosyasında DefaultConnection bağlantı dizesi bulunamadı.");
                    return;
                }

                var optionsBuilder = new DbContextOptionsBuilder<CryptoCurrencyDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                var hubUrl1 = "https://localhost:5001/cryptoCurrencyHub";
                var hubUrl2 = "https://localhost:6001/cryptoCurrencyHub";

                var connection1 = new HubConnectionBuilder()
                    .WithUrl(hubUrl1)
                    .Build();

                var connection2 = new HubConnectionBuilder()
                    .WithUrl(hubUrl2)
                    .Build();

                // signalr bağlantıları başlatılıyor
                await connection1.StartAsync();
                await connection2.StartAsync();

                var connectionId1 = await GetConnectionId(connection1);
                Console.WriteLine($"Connection ID 1: {connectionId1}");

                var connectionId2 = await GetConnectionId(connection2);
                Console.WriteLine($"Connection ID 2: {connectionId2}");

                var services = new ServiceCollection();
                services.AddHttpClient();

                var serviceProvider = services.BuildServiceProvider();
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();

                var thread = new Thread(async () =>
                {
                    while (isRunning)
                    {
                        try
                        {
                            var apiUrl = "http://localhost:5000/api/CryptoCurrency/GetCryptoCurrencies";
                            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                            response.EnsureSuccessStatusCode();

                            string responseBody = await response.Content.ReadAsStringAsync();

                            var cryptoCurrencies = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CryptoCurrency1>>(responseBody);

                            // Verileri işleyerek CryptoCurrency ve CryptoCurrencyValue nesneleri oluşturun
                            var cryptoCurrencyDict = new Dictionary<string, CryptoCurrency>();

                            using (var context = new CryptoCurrencyDbContext(optionsBuilder.Options))
                            {
                                // Inside the foreach loop in the thread
                                foreach (var crypto in cryptoCurrencies)
                                {
                                    if (crypto.Symbol != null && crypto.Name != null)
                                    {
                                        var symbol = crypto.Symbol;
                                        var name = crypto.Name;

                                        if (!cryptoCurrencyDict.ContainsKey(symbol))
                                        {
                                            cryptoCurrencyDict[symbol] = new CryptoCurrency
                                            {
                                                Symbol = symbol,
                                                Name = name,
                                                Values = new List<CryptoCurrencyValue>()
                                            };
                                        }

                                        var cryptoCurrencyValue = new CryptoCurrencyValue
                                        {
                                            Price = crypto.Price,
                                            Change24h = crypto.Change24h,
                                            Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"))
                                        };

                                        cryptoCurrencyDict[symbol].Values.Add(cryptoCurrencyValue);
                                    }
                                }

                                // Veritabanına ekleme işlemleri
                                foreach (var cryptoCurrency in cryptoCurrencyDict.Values)
                                {
                                    context.CryptoCurrencies.Add(cryptoCurrency);
                                    context.CryptoCurrencyValues.AddRange(cryptoCurrency.Values);
                                }

                                // Değişiklikleri kaydedin
                                await context.SaveChangesAsync();
                                await connection1.InvokeAsync("SendCryptoCurrencyData", cryptoCurrencies);
                                await connection2.InvokeAsync("SendCryptoCurrencyData", cryptoCurrencies);
                            }

                            Console.WriteLine("Veriler başarıyla alındı ve veritabanına kaydedildi.");

                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Hata: {ex.Message}");
                            Console.WriteLine($"StackTrace: {ex.StackTrace}");
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                    }
                });

                thread.Start();

                Console.WriteLine("Çıkış yapmak için bir tuşa basın.");
                Console.ReadKey();

                isRunning = false;
                thread.Join();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                Console.ReadKey();
            }
        }

        private static async Task<string> GetConnectionId(HubConnection connection)
        {
            return await connection.InvokeAsync<string>("GetConnectionId");
        }
    }
}
