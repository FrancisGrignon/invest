using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class StockService
{
    private readonly HttpClient _httpClient;
    private readonly DateTime _until;

    private const string ApiKey = ""; // Remplacez par votre cl� API Alpha Vantage
    private const string ApiUrl = "https://www.alphavantage.co/query";

    public StockService()
    {
        _httpClient = new HttpClient();
        _until = FindLastFriday();
    }

    public async Task UpdateStockValuesAsync(string symbol, string filePath)
    {
        // �tape 1 : R�cup�rer la valeur boursi�re
        var stockValue = await GetStockValueAsync(symbol);
        if (stockValue == null)
        {
            Console.WriteLine($"Impossible de r�cup�rer la valeur pour {symbol}");
            return;
        }

        // �tape 2 : Ajouter la valeur au fichier CSV
        AppendToCsv(filePath, stockValue.Value);
    }

    private async Task<decimal?> GetStockValueAsync(string symbol)
    {
        try
        {
            var query = $"{ApiUrl}?function=GLOBAL_QUOTE&symbol={symbol}&apikey={ApiKey}";
            var response = await _httpClient.GetAsync(query);

            // V�rifiez si la requ�te a r�ussi
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Erreur HTTP : {response.StatusCode} pour le symbole {symbol}");
                return null;
            }

            // Analysez la r�ponse JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(jsonResponse);

            if (jsonDocument.RootElement.TryGetProperty("Global Quote", out var globalQuote) &&
                globalQuote.TryGetProperty("05. price", out var priceElement) &&
                decimal.TryParse(priceElement.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                return price;
            }

            Console.WriteLine($"Donn�es manquantes ou mal format�es pour le symbole {symbol}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erreur de requ�te HTTP pour le symbole {symbol} : {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erreur d'analyse JSON pour le symbole {symbol} : {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur inattendue pour le symbole {symbol} : {ex.Message}");
            return null;
        }
    }

    private void AppendToCsv(string filePath, decimal stockValue)
    {
        var line = $"{_until:yyyy-MM-dd},{stockValue}";

        File.AppendAllText(filePath, line + Environment.NewLine);
    }

    private DateTime FindLastFriday()
    {
        var now = DateTime.Now.ToUniversalTime().Date;

        int backInTime;

        if (DayOfWeek.Friday == now.DayOfWeek)
        {
            // all good
            backInTime = 0;
        }
        else if (DayOfWeek.Friday < now.DayOfWeek)
        {
            backInTime = 1;
        }
        else
        {
            backInTime = Convert.ToInt32(now.DayOfWeek) + 2;
        }

        var friday = now.AddDays(-backInTime);

        return friday;
    }
}

