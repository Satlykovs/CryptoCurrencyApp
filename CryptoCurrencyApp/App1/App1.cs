namespace App1;


using System;
using System.Timers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class App1
{
    private static Timer timer;
    private static ExcelWriterService excelWriterService;

    public static async Task Main(string[] args)
    {

        excelWriterService = new ExcelWriterService();
        Console.Write("Укажите название файла для сохранения данных : ");

        string filePath = Console.ReadLine();
        const string url = "https://api.coindesk.com/v1/bpi/currentprice.json";

        ParseCryptoCurrencyDataAsync(url, filePath);
        Console.WriteLine(DateTime.Now.ToString() + ": Выполнение парсинга данных...");

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
        Console.WriteLine(DateTime.Now.ToString() + ": Выполнение парсинга данных...");

    }

    public static async Task ParseCryptoCurrencyDataAsync(string url, string filePath)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                CryptoCurrencyData data = JsonSerializer.Deserialize<CryptoCurrencyData>(responseContent);
                List<string> headers = ["Время обновления", "Название криптовалюты", "Символ",
                 "Цена (USD)", $"Дата последнего обновления таблицы : {DateTime.Now.ToString()}"];
                filePath = Regex.Replace(filePath, @"\..*", "") + ".csv";
                excelWriterService.WriteToCsv(filePath, headers, ConvertDataToList(data));
            }
        }
    }



    public static List<string> ConvertDataToList(CryptoCurrencyData cryptocurrencyData)
    {
        string time = cryptocurrencyData.time.updated;
        string name = cryptocurrencyData.chartName;
        string symbol = name.Substring(0, 1) + Regex.Replace(name.ToUpper().Substring(1), "(?i)[AEIOUY]","").Substring(0, 2);
        string price = cryptocurrencyData.bpi.USD.rate_float.ToString();

        return [time, name, symbol, price];
    }
}
