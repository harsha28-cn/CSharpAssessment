using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CSharpAssessment.Models;

namespace CSharpAssessment.Services
{
    public class ApiService
    {
        private static readonly string apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

        public static async Task<List<TimeEntry>> FetchTimeEntriesAsync()
        {
            using HttpClient client = new HttpClient();
            var json = await client.GetStringAsync(apiUrl);

            var entriesRaw = JsonSerializer.Deserialize<List<TimeEntryRaw>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (entriesRaw == null)
                throw new Exception("Failed to parse JSON data from API.");

            // Group and calculate total hours worked
            var grouped = entriesRaw
                .Where(e => !string.IsNullOrEmpty(e.EmployeeName) && e.StarTimeUtc != null && e.EndTimeUtc != null)
                .GroupBy(e => e.EmployeeName)
                .Select(g => new TimeEntry
                {
                    EmployeeName = g.Key,
                    HoursWorked = g.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours)
                })
                .OrderByDescending(e => e.HoursWorked)
                .ToList();

            return grouped;
        }
    }

    public class TimeEntryRaw
    {
        public string EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
    }
}
