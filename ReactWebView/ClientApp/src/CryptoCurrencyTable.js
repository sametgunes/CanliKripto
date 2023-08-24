import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

const CryptoCurrencyTable = () => {
    const [cryptoCurrencies, setCryptoCurrencies] = useState([]);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/cryptoCurrencyHub")
            .build();

        connection.start()
            .then(() => {
                console.log("SignalR connection started.");

                connection.on("ReceiveCryptoCurrencyData", (cryptoCurrencies) => {
                    const updatedData = cryptoCurrencies.map(crypto => {
                        const existingCrypto = cryptoCurrencies.find(item => item.symbol === crypto.symbol);

                        if (existingCrypto) {
                            return {
                                ...existingCrypto,
                                price: crypto.price,
                                change24h: crypto.change24h,
                                date: new Date().toLocaleString()
                            };
                        }

                        return crypto;
                    });

                    setCryptoCurrencies(updatedData);
                });

                fetchCryptoCurrencyData();
            })
            .catch(error => console.error(error));

        function fetchCryptoCurrencyData() {
            connection.invoke("SendCryptoCurrencyData", []).catch(function (err) {
                console.error(err.toString());
            });
        }

        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div>
            <h1>Crypto Currency Table With React</h1>
            <table>
                <thead>
                    <tr>
                        <th>Symbol</th>
                        <th>Name</th>
                        <th>Price</th>
                        <th>Change 24h</th>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {cryptoCurrencies.map(crypto => (
                        <tr key={crypto.symbol}>
                            <td>{crypto.symbol}</td>
                            <td>{crypto.name}</td>
                            <td>{crypto.price}</td>
                            <td>{crypto.change24h}</td>
                            <td>{crypto.date}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <style>{`
        table {
          width: 100%;
          border-collapse: collapse;
        }
        th, td {
          padding: 8px;
          text-align: left;
          border-bottom: 1px solid #ddd;
        }
        th {
          background-color: #f2f2f2;
          font-weight: bold;
        }
        tr:hover {
          background-color: #f5f5f5;
        }
      `}</style>
        </div>
    );
};

export default CryptoCurrencyTable;
