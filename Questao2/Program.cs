using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

public class Program
{
    public const string API_URL = "https://jsonmock.hackerrank.com/api/football_matches";
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddTransient<HttpClient>();
                services.AddTransient<IGames, Games>();

            })
            .Build();

        var svc = ActivatorUtilities.CreateInstance<Games>(host.Services);
        svc.Run().GetAwaiter().GetResult();
    }

    public interface IGames
    {
        Task Run();
        string BuildUrl(int? year = null, string team1 = null, string team2 = null, int? page = null);
        Task<int> GetTotalScoredGoalsAsync(string team, int year);
    }

    public class Games : IGames
    {
        private HttpClient _httpClient;
        public Games(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Run()
        {
            string teamName = "Paris Saint-Germain";
            int year = 2013;
            int totalGoals = await GetTotalScoredGoalsAsync(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

            teamName = "Chelsea";
            year = 2014;
            totalGoals = await GetTotalScoredGoalsAsync(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

            // Output expected:
            // Team Paris Saint - Germain scored 109 goals in 2013
            // Team Chelsea scored 92 goals in 2014
        }

        public async Task<int> GetTotalScoredGoalsAsync(string team, int year)
        {
            int totalGoals = 0;
            int page = 1;

            // Consultar gols como `team1`
            while (true)
            {
                var urlTeam1 = BuildUrl(year, team, null, page);
                var result = await FetchGoalsFromPage(urlTeam1, "team1goals");

                if (result.Item2 == 0) break; // Sem mais páginas
                totalGoals += result.Item2;
                page++;
            }

            return totalGoals;
        }

        private async Task<(int, int)> FetchGoalsFromPage(string url, string goalField)
        {
            var response = await _httpClient.GetStringAsync(url);
            var jsonDoc = JsonDocument.Parse(response);

            // Verifica o total de páginas
            int totalPages = jsonDoc.RootElement.GetProperty("total_pages").GetInt32();
            int goalsInPage = 0;

            foreach (var match in jsonDoc.RootElement.GetProperty("data").EnumerateArray())
            {
                goalsInPage += Convert.ToInt32(match.GetProperty(goalField).GetString());
            }

            return (totalPages, goalsInPage);
        }

        public string BuildUrl(int? year = null, string team1 = null, string team2 = null, int? page = null)
        {
            var baseUrl = API_URL;
            var queryString = new StringBuilder();

            if (year.HasValue)
                queryString.Append($"year={year.Value}&");

            if (!string.IsNullOrEmpty(team1))
                queryString.Append($"team1={Uri.EscapeDataString(team1)}&");

            if (!string.IsNullOrEmpty(team2))
                queryString.Append($"team2={Uri.EscapeDataString(team2)}&");

            if (page.HasValue)
                queryString.Append($"page={page.Value}&");

            if (queryString.Length > 0)
                queryString.Length--;

            return queryString.Length > 0 ? $"{baseUrl}?{queryString}" : baseUrl;
        }

    }
}