using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CSharpAssessment.Models;

namespace CSharpAssessment.Services
{
    public class HtmlGenerator
    {
        public static void GenerateHtmlTable(List<TimeEntry> entries, string filePath)
        {
            double total = entries.Sum(x => x.HoursWorked);
            StringBuilder html = new StringBuilder();

            html.AppendLine("<html><head><style>");
            html.AppendLine("table { border-collapse: collapse; width: 60%; }");
            html.AppendLine("th, td { border: 1px solid black; padding: 8px; text-align: left; }");
            html.AppendLine("</style></head><body>");
            html.AppendLine("<h2>Employee Hours Worked</h2>");
            html.AppendLine("<table><tr><th>Name</th><th>Total Hours</th><th>Percentage</th></tr>");

            // Final color map from the current pie chart
            Dictionary<string, string> colorMap = new()
            {
                { "Patrick Huthinson", "#FFFFFF" },  // White
                { "Stewart Malachi",    "#A9A9A9" }, // Gray
                { "John Black",         "#BDB76B" }, // DarkKhaki
                { "Abhay Singh",        "#FFC080" }, // Peach
                { "Mary Poppins",       "#800080" }, // Purple
                { "Tim Perkinson",      "#D2B48C" }, // Tan
                { "Kavvay Verma",       "#A52A2A" }, // Brown
                { "Rita Alley",         "#FF00FF" }, // Magenta
                { "Raju Sunuwar",       "#FF0000" }, // Red
                { "Tamoy Smith",        "#FF0000" }  // Red
            };

            foreach (var entry in entries)
            {
                string hexColor = colorMap.ContainsKey(entry.EmployeeName)
                    ? colorMap[entry.EmployeeName]
                    : "#DDDDDD"; // fallback gray

                Color bgColor = ColorTranslator.FromHtml(hexColor);
                string textColor = IsDark(bgColor) ? "white" : "black";

                double percent = (entry.HoursWorked / total) * 100.0;

                html.AppendLine($"<tr style='background-color: {hexColor}; color: {textColor};'>" +
                                $"<td>{entry.EmployeeName}</td><td>{entry.HoursWorked:F2}</td><td>{percent:F1}%</td></tr>");
            }

            html.AppendLine("</table></body></html>");
            File.WriteAllText(filePath, html.ToString());
        }

        private static bool IsDark(Color color)
        {
            double brightness = (color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
            return brightness < 150;
        }
    }
}
