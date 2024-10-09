using System;
using System.IO;
using BusdriverBreaksConsoleUI;

class Program
{
    private static string _filePath = "";

    static void Main(string[] args)
    {

        _filePath = args.Length > 1 ? args[1] : "";
        if (args.Length > 1 && args[0] == "filename")
        {
            HandleFile();
        }
        else {
            Menu mainMenu = new Menu("Busdriver Breaks", EMenuLevel.Root);
            mainMenu.AddMenuItems(new List<MenuItem>()
            {
                new("I", "Insert drivers breaks", HandleInput),
                // new("R", "Previous Results", See Previous Results)
               
            });
            mainMenu.Run();
        }
    }

    static int ConvertTimeToMinutes(string time)
    {
        return (int.Parse(time.Split(':')[0]) * 60) + int.Parse(time.Split(':')[1]);
    }

    static string ConvertMinutesToTime(int minutes)
    {
        return $"{minutes / 60:D2}:{minutes % 60:D2}";
    }

    static int StringToMinutesNumber(string time)
    {
        var parts = time.Split(':');
        return int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
    }

    static void HandleFile()
    {
        var breaks = new List<(string, string)>();
        if (File.Exists(_filePath))
        {
            foreach (var line in File.ReadLines(_filePath))
            {
                var times = ParseTime(line);
                if (times != null)
                {
                    breaks.Add(times.Value);
                }
                else 
                { 
                    Console.WriteLine("Invalid format. Please make sure time is in format: HH:MMHH:MM.");
                    return;
                }
            }
            ShowBusiestPeriod(breaks);
        }
        else
        {
            Console.WriteLine("File was not found.");
        }
        return;
    }

    static string HandleInput()
    {
        var breaks = new List<(string, string)>();
        
        Console.WriteLine("Enter driver break times. Time should be in format: HH:MMHH:MM format (e.g., 10:1511:30). Press escape key to quit");

        while (Console.ReadKey().Key != ConsoleKey.Escape)
        {
            Console.WriteLine("Add break: ");
            var input = Console.ReadLine()?.Trim();

            var times = ParseTime(input);
            if (times != null)
            {
                breaks.Add(times.Value);
                ShowBusiestPeriod(breaks);
            }
            else
            {
                Console.WriteLine("Invalid format. Please enter time as HH:MMHH:MM.");
            }
        }
        return "";
    }

    static (string start, string end)? ParseTime(string entry)
    {
        if (entry.Length == 10)
        {
            // return starttime and endtime as tuple
            return (entry.Substring(0, 5), entry.Substring(5));
        }
        return null;
    }

    static void ShowBusiestPeriod(List<(string start, string end)> breaks)
    {
        var breakTimes = new Dictionary<string, int>();

        foreach (var (start, end) in breaks)
        {
            var breakStart = ConvertTimeToMinutes(start);
            var breakEnd = ConvertTimeToMinutes(end);

            for (var time = breakStart; time <= breakEnd; time++)
            {
                var breakTime = ConvertMinutesToTime(time);
                if (!breakTimes.ContainsKey(breakTime))
                {
                    breakTimes[breakTime] = 0;
                }
                breakTimes[breakTime]++;
            }
        }

        // Order by busiest break periods
        var busiestTime = breakTimes.OrderByDescending(t => t.Value).First();
        var busiestPeriodStart = busiestTime.Key;
        var busiestPeriodEnd = busiestTime.Key;

        foreach (var time in breakTimes.Keys.OrderBy(t => t))
        {
            // Search till next time is not as busy
            if (breakTimes[time] == busiestTime.Value)
            {
                busiestPeriodEnd = time;
            }
            else if (breakTimes[time] < busiestTime.Value && StringToMinutesNumber(time) > StringToMinutesNumber(busiestPeriodEnd))
            {
                break;
            }
        }

        Console.WriteLine($"Busiest break period is {busiestPeriodStart}-{busiestPeriodEnd} with {busiestTime.Value} drivers resting.");
    }
}