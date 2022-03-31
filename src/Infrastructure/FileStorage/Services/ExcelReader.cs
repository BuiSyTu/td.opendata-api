using Newtonsoft.Json.Linq;
using Syncfusion.XlsIO;
using System.Text;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public class ExcelReader : IExcelReader
{
    public string? GetJsonData(Stream? stream, int? sheetNumber)
    {
        ExcelEngine excelEngine = new();

        // Instantiate the Excel application object.
        IApplication application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Xlsx;

        IWorkbook book = application.Workbooks.Open(stream);
        stream.Close();

        // Access worksheet.
        int workSheetNumber = sheetNumber ?? 0;
        IWorksheet worksheet = book.Worksheets[workSheetNumber];

        MemoryStream jsonStream = new();
        book.SaveAsJson(jsonStream, worksheet);

        excelEngine.Dispose();

        byte[] json = new byte[jsonStream.Length];

        // Read the JSON stream and convert to a JSON object.
        jsonStream.Position = 0;
        jsonStream.Read(json, 0, (int)jsonStream.Length);
        return Encoding.UTF8.GetString(json);
    }

    public string? GetJsonData(Stream? stream, string? sheetName)
    {
        ExcelEngine excelEngine = new();

        // Instantiate the Excel application object.
        IApplication application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Xlsx;

        IWorkbook book = application.Workbooks.Open(stream);
        stream.Close();

        // Access first worksheet.
        IWorksheet worksheet = !string.IsNullOrEmpty(sheetName)
            ? book.Worksheets[sheetName]
            : book.Worksheets[0];

        MemoryStream jsonStream = new();
        book.SaveAsJson(jsonStream, worksheet);

        excelEngine.Dispose();

        byte[] json = new byte[jsonStream.Length];

        // Read the JSON stream and convert to a JSON object.
        jsonStream.Position = 0;
        jsonStream.Read(json, 0, (int)jsonStream.Length);
        return Encoding.UTF8.GetString(json);
    }
}
