"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/cryptoCurrencyHub").build();


connection.start()
    .then(() => {
        console.log("SignalR bağlantısı başlatıldı.");

        // Connection ID'yi kullanarak istediğiniz işlemi yapabilirsiniz
        console.log("Connection ID: " + connection.connectionId);

        // Verileri ilk kez yükle
        fetchCryptoCurrencyData();

        // Belirli bir süre aralığında verileri güncelle
        const refreshInterval = 6000;
        setInterval(fetchCryptoCurrencyData, refreshInterval);
    })
    .catch(error => console.error(error));


connection.on("ReceiveCryptoCurrencyData", function (cryptoCurrencies) {
    // Tabloyu güncelle
    updateTableWithData(cryptoCurrencies);
});

function updateTableWithData(cryptoCurrencies) {
    const tableBody = document.getElementById("cryptoCurrencyTableBody");
    tableBody.innerHTML = "";

    for (let i = 0; i < cryptoCurrencies.length; i++) {
        const crypto = cryptoCurrencies[i];
        const symbol = crypto.Symbol;
        const name = crypto.Name;
        const price = crypto.CurrentPrice;
        const change24h = crypto.PriceChangePercentage24h;
        const date = new Date().toLocaleString();

        const row = "<tr>" +
            "<td>" + symbol + "</td>" +
            "<td>" + name + "</td>" +
            "<td>" + price + "</td>" +
            "<td>" + change24h + "</td>" +
            "<td>" + date + "</td>" +
            "</tr>";

        tableBody.innerHTML += row;
    }
}

function fetchCryptoCurrencyData() {
    // Sunucudan verileri iste
    connection.invoke("SendCryptoCurrencyData", []).catch(function (err) {
        return console.error(err.toString());
    });
}