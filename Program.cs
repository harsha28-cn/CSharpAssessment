using System;
using System.Threading.Tasks;
using CSharpAssessment.Services;
using System.IO;

class Program
{
    static async Task Main()
    {
        var entries = await ApiService.FetchTimeEntriesAsync();

        Directory.CreateDirectory("Output");

        string htmlPath = Path.Combine("Output", "employees.html");
        HtmlGenerator.GenerateHtmlTable(entries, htmlPath);
        Console.WriteLine($"✅ HTML table generated: {htmlPath}");

        string chartPath = Path.Combine("Output", "piechart.png");
        PieChartGenerator.GeneratePieChart(entries, chartPath);
        Console.WriteLine($"✅ Pie chart generated: {chartPath}");
    }
}
