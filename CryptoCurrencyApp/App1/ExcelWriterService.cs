using System.Text;

namespace App1;

public class ExcelWriterService
{
    public void WriteToCsv(string filePath, List<string> headers, List<string> data)
    {

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            WriteCsvLine(writer, headers);
            WriteCsvLine(writer, data);
        }
    }

    public void WriteCsvLine(StreamWriter writer, List<string> data)
    {
        writer.WriteLine(string.Join(";", data));
    }
}