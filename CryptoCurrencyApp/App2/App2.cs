namespace App2;


using System;
using System.Timers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class App2
{
    private static Timer timer;
    private static ExcelWriterService excelWriterService;

    public static async Task Main(string[] args)
    {

        excelWriterService = new ExcelWriterService();

        Console.Write("Укажите название файла для сохранения данных : ");

        string filePath = Console.ReadLine();
        const string url = "https://api.coinlore.net/api/tickers/";
        
        ParseCryptoCurrencyDataAsync(url, filePath);
        Console.WriteLine(DateTime.Now.ToString() + " : Выполнение парсинга данных...");


        timer = new Timer(3600000); 
        timer.Elapsed += async (sender, e) => await OnTimedEventAsync(url, filePath);
        timer.AutoReset = true;
        timer.Enabled = true;


        Console.WriteLine("Таймер запущен. Нажмите Enter для завершения.");
        Console.ReadLine();
    }

    public static async Task OnTimedEventAsync(string url, string filePath)
    {
        await ParseCryptoCurrencyDataAsync(url, filePath);
        Console.WriteLine(DateTime.Now.ToString() + " : Выполнение парсинга данных...");

    }

    public static async Task ParseCryptoCurrencyDataAsync(string url, string filePath)
    {
        using (HttpClient client = new HttpClient())
        {

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {

                string responseContent = await response.Content.ReadAsStringAsync();
                CoinLoreResponse data = JsonSerializer.Deserialize<CoinLoreResponse>(responseContent);
                List<string> dataList = await ConvertDataToList(data);
            
                string headers = "ID;Название криптовалюты;Символ;Цена (USD);Цена (BTC);" +
                 "Изменение цены за последний час;Изменение цены за день;Изменение" +
                 "цены за неделю;Объем торгов за день (USD);Средний объем торгов (USD);" +
                 "Рыночная капитализация (USD);Дата последнего обновления таблицы : " +
                 $"{DateTime.Now}";
                
                filePath = Regex.Replace(filePath, @"\..*", "") + ".csv";
                excelWriterService.WriteToCsv(filePath, headers, dataList);
            }
        }
    }


    public static async Task<List<string>> ConvertDataToList(CoinLoreResponse cryptocurrencyData)
    {
        List<string> dataList = [];
        foreach(var coin in cryptocurrencyData.data)
        {
            string coinData = $"{coin.id};{coin.name};{coin.symbol};{coin.price_usd};" + 
                $"{coin.price_btc};{coin.percent_change_1h};{coin.percent_change_24h};" + 
                $"{coin.percent_change_7d};{coin.volume24};{coin.volume24a};{coin.market_cap_usd}";
            dataList.Add(coinData);
        }
        return dataList;
    }
}
