# 📊 C# Assessment Project - Employee Hours Visualization

This C# console application fetches employee working hour data from an API, and generates:

- ✅ A color-coded **HTML table** (`employees.html`) showing hours worked and percentage.
- ✅ A **Pie Chart** (`piechart.png`) that visually represents time distribution.
- ✅ Color mapping is consistent across both the table and the chart.

---

## 📁 Project Structure

CSharpAssessment/
│
├── Models/
│ └── TimeEntry.cs # Employee data model
│
├── Services/
│ ├── ApiService.cs # Simulates API fetch (can be real or mock)
│ ├── HtmlGenerator.cs # Creates the employees.html file
│ └── PieChartGenerator.cs # Creates the piechart.png file
│
├── Output/
│ ├── employees.html # Generated HTML table
│ └── piechart.png # Generated pie chart image
│
├── Program.cs # Main entry point
├── CSharpAssessment.csproj # .NET project file

---

## ▶️ How to Run the Project

1. **Install .NET SDK**  
   Make sure [.NET 6.0 SDK or newer](https://dotnet.microsoft.com/download) is installed.

2. **Clone the Repository**

```bash
git clone https://github.com/your-username/CSharpAssessment.git
cd CSharpAssessment


BUILD and RUN
dotnet build
dotnet run
