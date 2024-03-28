using System.Text;

namespace App2;

public class ExcelWriterService
{
    public void WriteToCsv(string filePath, string headers, List<string> data)
    {

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            WriteCsvLine(writer, headers);
            WriteCsvLines(writer, data);
        }
    }

    public void WriteCsvLines(StreamWriter writer, List<string> data)
    {
        foreach (string coin in data)
        {
            WriteCsvLine(writer, coin);
        }
    }

    public void WriteCsvLine(StreamWriter writer, string data)
    {
        writer.WriteLine(data);
    }
}