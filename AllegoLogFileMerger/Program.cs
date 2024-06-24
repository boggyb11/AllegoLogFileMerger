
using System.Globalization;

string outputFilePath;
List<string> inputLogFilePaths = [];
List<Log> logs = [];

if (args.Length < 2)
{
    outputFilePath = "../../../LogOutput.txt";
    inputLogFilePaths.AddRange(["../../../LogInput1.txt", "../../../LogInput2.txt"]);
    //Ask user for locations
}
else
{
    outputFilePath = args[0];
    inputLogFilePaths = args.Skip(1).ToList();
}

foreach (var filePath in inputLogFilePaths)
{
    if (!File.Exists(filePath)) Console.WriteLine($"File not found: {filePath}");

    foreach (var line in File.ReadLines(filePath))
    {
        Log log = ParseLog(line);
        if (log != null)
        {
            logs.Add(log);
        }
    }
}

var sortedLogs = logs.OrderBy(x => x.Timestamp).ToList();
using (StreamWriter sr = new(outputFilePath))
{
    foreach (var log in sortedLogs)
    {
        sr.WriteLine($"{log.Timestamp} {log.Message}");
    }
}

Console.WriteLine("Contents written to ouput file");

static Log ParseLog(string log)
{
    try
    {
        var splitLog = log.Split(' ');

        DateTime timestamp = DateTime.ParseExact(splitLog[0] + " " + splitLog[1], "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        var message = string.Join(" ", splitLog.Skip(2));

        return new Log
        {
            Timestamp = timestamp,
            Message = message
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to parse log: {log}. Error: {ex.Message}");
        return null;
    }
}

class Log
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
}