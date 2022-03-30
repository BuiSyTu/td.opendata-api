using Newtonsoft.Json.Linq;
using Syncfusion.XlsIO;
using System.Text;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public class ExcelReader : IExcelReader
{
    private string GetJsonString(Stream? stream, int? sheetNumber)
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

    private string GetJsonString(Stream? stream, string? sheetName)
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

    public MetadataCollection GetMetadata(Stream? stream, string? sheetName)
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

        IRange usedRange = worksheet.UsedRange;

        int lastColumn = usedRange.LastColumn;

        excelEngine.Dispose();

        MetadataCollection metadatas = new();
        for (int col = 1; col < lastColumn; col++)
        {
            metadatas.Add(new Metadata
            {
                Data = worksheet[1, col].Value
            });
        }

        return metadatas;
    }

    public MetadataCollection GetMetadata(Stream? stream, int? sheetNumber)
    {
        ExcelEngine excelEngine = new();

        // Instantiate the Excel application object.
        IApplication application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Xlsx;

        IWorkbook book = application.Workbooks.Open(stream);
        stream.Close();

        // Access first worksheet.
        IWorksheet worksheet = book.Worksheets[sheetNumber ?? 0];

        IRange usedRange = worksheet.UsedRange;

        int lastColumn = usedRange.LastColumn;

        MetadataCollection metadatas = new();
        for (int col = 1; col <= lastColumn; col++)
        {
            metadatas.Add(new Metadata
            {
                Data = worksheet[1, col].Value,
                Type = "string",
                IsDisplay = true
            });
        }

        excelEngine.Dispose();
        return metadatas;
    }

    public PreviewData GetData(string? url)
    {
        throw new NotImplementedException();
    }

    public PreviewData GetData(Stream? stream, int? sheetNumber)
    {
        string jsonString = GetJsonString(stream, sheetNumber);
        return jsonString.ToPreviewData();
    }

    public PreviewData GetData(Stream? stream, string? sheetName)
    {
        string jsonString = GetJsonString(stream, sheetName);
        return jsonString.ToPreviewData();
    }
}
