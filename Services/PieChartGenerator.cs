using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using CSharpAssessment.Models;

namespace CSharpAssessment.Services
{
    public class PieChartGenerator
    {
        public static void GeneratePieChart(List<TimeEntry> entries, string filePath)
        {
            int width = 700, height = 700;
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            float total = (float)entries.Sum(x => x.HoursWorked);
            float startAngle = 0f;
            Font font = new Font("Arial", 11, FontStyle.Bold);

            // Pie chart position and size
            int pieX = 150, pieY = 150, pieSize = 400;
            int centerX = pieX + pieSize / 2;
            int centerY = pieY + pieSize / 2;
            int radius = pieSize / 2;

            // Define fixed color map
            Dictionary<string, Brush> colorMap = new()
            {
                { "Patrick Huthinson", new SolidBrush(Color.White) },
                { "Stewart Malachi",    new SolidBrush(Color.FromArgb(169, 169, 169)) }, // DarkGray
                { "John Black",         new SolidBrush(Color.FromArgb(189, 183, 107)) }, // DarkKhaki
                { "Abhay Singh",        new SolidBrush(Color.FromArgb(255, 192, 128)) }, // LightOrange
                { "Mary Poppins",       new SolidBrush(Color.FromArgb(128, 0, 128)) },   // Purple
                { "Tim Perkinson",      new SolidBrush(Color.FromArgb(210, 180, 140)) }, // Tan
                { "Kavvay Verma",       new SolidBrush(Color.FromArgb(165, 42, 42)) },   // Brown
                { "Rita Alley",         new SolidBrush(Color.Magenta) },
                { "Raju Sunuwar",       new SolidBrush(Color.Red) },
                { "Tamoy Smith",        new SolidBrush(Color.Red) }
            };

            // Track red slice angles
            float? rajuStart = null, tamoyStart = null;

            // Draw pie slices with percentage inside
            foreach (var entry in entries)
            {
                float sweep = (float)(entry.HoursWorked / total) * 360f;
                Brush brush = colorMap.ContainsKey(entry.EmployeeName)
                    ? colorMap[entry.EmployeeName]
                    : new SolidBrush(GetConsistentColor(entry.EmployeeName));

                g.FillPie(brush, pieX, pieY, pieSize, pieSize, startAngle, sweep);

                // Calculate center angle for label
                float midAngle = startAngle + sweep / 2f;
                double rad = midAngle * Math.PI / 180;
                int textX = centerX + (int)(radius * 0.6 * Math.Cos(rad));
                int textY = centerY + (int)(radius * 0.6 * Math.Sin(rad));

                float percent = (float)(entry.HoursWorked / total) * 100f;
                string percentLabel = $"{percent:F1}%";

                // Text shadow for better contrast
                g.DrawString(percentLabel, font, Brushes.White, textX + 1, textY + 1);
                g.DrawString(percentLabel, font, Brushes.Black, textX, textY);

                // Track slice for white separator
                if (entry.EmployeeName == "Raju Sunuwar")
                    rajuStart = startAngle + sweep;
                if (entry.EmployeeName == "Tamoy Smith")
                    tamoyStart = startAngle;

                startAngle += sweep;
            }

            // Draw white separator line between Raju and Tamoy
            if (rajuStart.HasValue && tamoyStart.HasValue)
            {
                float separatorAngle = (rajuStart.Value + tamoyStart.Value) / 2f;
                double rad = separatorAngle * Math.PI / 180;
                int endX = centerX + (int)(radius * Math.Cos(rad));
                int endY = centerY + (int)(radius * Math.Sin(rad));
                g.DrawLine(new Pen(Color.White, 2), centerX, centerY, endX, endY);
            }

            // Draw black border around pie
            g.DrawEllipse(Pens.Black, pieX, pieY, pieSize, pieSize);

            // Draw legend
            float labelX = 10;
            float labelY = 10;

            foreach (var entry in entries)
            {
                Brush brush = colorMap.ContainsKey(entry.EmployeeName)
                    ? colorMap[entry.EmployeeName]
                    : new SolidBrush(GetConsistentColor(entry.EmployeeName));

                float percent = (float)(entry.HoursWorked / total) * 100f;

                g.FillRectangle(brush, labelX, labelY, 15, 15);
                g.DrawRectangle(Pens.Black, labelX, labelY, 15, 15);
                g.DrawString($"{entry.EmployeeName} ({entry.HoursWorked:F1}h – {percent:F1}%)", font, Brushes.Black, labelX + 20, labelY);
                labelY += 20;
            }

            bmp.Save(filePath, ImageFormat.Png);
        }

        // Consistent fallback color generator
        private static Color GetConsistentColor(string key)
        {
            int hash = key.GetHashCode();
            Random rand = new Random(hash);
            return Color.FromArgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256));
        }
    }
}
