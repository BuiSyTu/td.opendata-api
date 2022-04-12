using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TD.OpenData.WebApi.Application.FileStorage;
using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Infrastructure.Common.Extensions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.FileStorage;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType)
    where T : class
    {
        if (request == null || request.Data == null)
        {
            return Task.FromResult(string.Empty);
        }

        if (request.Extension is null || !supportedFileType.GetDescriptionList().Contains(request.Extension))
            throw new InvalidOperationException("File Format Not Supported.");
        if (request.Name is null)
            throw new InvalidOperationException("Name is required.");

        string base64Data = Regex.Match(request.Data, "data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
        if (streamData.Length > 0)
        {
            string folder = typeof(T).Name;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                folder = folder.Replace(@"\", "/");
            }

            string folderName = supportedFileType switch
            {
                FileType.Image => Path.Combine("Files", "Images", folder),
                _ => Path.Combine("Files", "Others", folder),
            };
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            bool exists = Directory.Exists(pathToSave);
            if (!exists)
            {
                Directory.CreateDirectory(pathToSave);
            }

            string fileName = request.Name.Trim('"');
            fileName = RemoveSpecialCharacters(fileName);
            fileName = fileName.ReplaceWhitespace("-");
            fileName += request.Extension.Trim();
            string fullPath = Path.Combine(pathToSave, fileName);
            string dbPath = Path.Combine(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            using var stream = new FileStream(fullPath, FileMode.Create);
            streamData.CopyTo(stream);
            dbPath = dbPath.Replace("\\", "/");
            return Task.FromResult("{server_url}/" + dbPath);
        }
        else
        {
            return Task.FromResult(string.Empty);
        }
    }

    public Task<List<AttachmentDto>> UploadFilesAsync<T>(CreateAttachmentRequest? request)
    where T : class
    {
        List<AttachmentDto> listFile = new List<AttachmentDto>();
        var files = request.Files;
        long size = files.Sum(f => f.Length);

        if (files.Any(f => f.Length == 0))
        {
            throw new InvalidOperationException("File Not Found.");
        }

        string folder = typeof(T).Name;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            folder = folder.Replace(@"\", "/");
        }

        string folderName = Path.Combine("Files", "Others", folder);

        string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        bool exists = Directory.Exists(pathToSave);
        if (!exists)
        {
            Directory.CreateDirectory(pathToSave);
        }

        foreach (var formFile in files)
        {
            if (formFile.Length > 0)
            {
                string? fileName = formFile.FileName.Trim('"');
                fileName = RemoveSpecialCharacters(fileName);
                fileName = fileName.ReplaceWhitespace("-");

                Guid dir_UUID = Guid.NewGuid();
                string dir_UUID_String = dir_UUID.ToString();

                string? target = Path.Combine(pathToSave, dir_UUID_String);
                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }

                string? fullPath = Path.Combine(target, fileName);
                string? dbPath = Path.Combine(folderName, dir_UUID_String, fileName);

                if (File.Exists(dbPath))
                {
                    dbPath = NextAvailableFilename(dbPath);
                    fullPath = NextAvailableFilename(fullPath);
                }

                using var stream = new FileStream(fullPath, FileMode.Create);
                formFile.CopyTo(stream);
                dbPath = dbPath.Replace("\\", "/");

                var attachment = new AttachmentDto
                {
                    Name = fileName,
                    Type = Path.GetExtension(formFile.FileName),
                    Url = dbPath
                };
                listFile.Add(attachment);
            }
        }

        return Task.FromResult(listFile);
    }

    public static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
    }

    public void Remove(string? path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private const string NumberPattern = "-{0}";

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
        {
            return path;
        }

        if (Path.HasExtension(path))
        {
            return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));
        }

        return GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        string tmp = string.Format(pattern, 1);

        if (!File.Exists(tmp))
        {
            return tmp;
        }

        int min = 1, max = 2;

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
            {
                min = pivot;
            }
            else
            {
                max = pivot;
            }
        }

        return string.Format(pattern, max);
    }

}