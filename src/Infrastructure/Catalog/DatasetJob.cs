using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.Notifications;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using Microsoft.Extensions.Localization;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Dapper;
using System.Data;
using System;
using System.ComponentModel;
using System.Collections;

namespace TD.OpenData.WebApi.Infrastructure.Catalog;

public class DatasetJob : IDatasetJob
{
    private readonly IEventService _eventService;
    private readonly ILogger<DatasetJob> _logger;
    private readonly IStringLocalizer<DatasetJob> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;
    private readonly ISerializerService _serializerService;

    public DatasetJob(
        ILogger<DatasetJob> logger,
        IRepositoryAsync repository,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationService notificationService,
        ICurrentUser currentUser,
        IStringLocalizer<DatasetJob> localizer,
        ISerializerService serializerService,
        IEventService eventService)
    {
        _logger = logger;
        _repository = repository;
        _progressBar = progressBar;
        _performingContext = performingContext;
        _notificationService = notificationService;
        _currentUser = currentUser;
        _progress = _progressBar.Create();
        _eventService = eventService;
        _localizer = localizer;
        _serializerService = serializerService;
    }

    private async Task Notify(string message, int progress = 0)
    {
        _progress.SetValue(progress);
        await _notificationService.SendMessageToUserAsync(
            _currentUser.GetUserName(),
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            });
    }

    [Queue("notdefault")]
    public async Task GenerateAsync(int nSeed)
    {
        await Notify("Your job processing has started");
        foreach (int index in Enumerable.Range(1, nSeed))
        {
            await _repository.CreateAsync(new Brand(name: $"Brand Random - {Guid.NewGuid()}", "Funny description"));
            await Notify("Progress: ", nSeed > 0 ? (index * 100 / nSeed) : 0);
        }

        await _repository.SaveChangesAsync();
        await _eventService.PublishAsync(new StatsChangedEvent());
        await Notify("Job successfully completed");
    }

    [Queue("notdefault")]
    [AutomaticRetry(Attempts = 5)]
    public async Task CleanAsync()
    {
        _logger.LogInformation("Initializing Job with Id: {JobId}", _performingContext.BackgroundJob.Id);
        var items = await _repository.GetListAsync<Brand>(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains("Brand Random"));
        _logger.LogInformation("Brands Random: {BrandsCount} ", items.Count.ToString());

        foreach (var item in items)
        {
            await _repository.RemoveAsync(item);
        }

        int rows = await _repository.SaveChangesAsync();
        await _eventService.PublishAsync(new StatsChangedEvent());
        _logger.LogInformation("Rows affected: {rows} ", rows.ToString());
    }

    [Queue("notdefault")]
    public async Task DatasetWebAPIAsync(Guid idDataset)
    {
        await Notify("Dữ liệu bắt đầu được đồng bộ");

        try
        {
            var dataset = await _repository.GetByIdAsync<Dataset>(idDataset);
            if (dataset == null)
            {
                await Notify("Lỗi, dữ liệu chưa được đồng bộ!");
                throw new EntityNotFoundException(string.Format(_localizer["dataset.notfound"], idDataset));
            }

            var datasetAPIConfigs = await _repository.GetListAsync<DatasetAPIConfig>(x => x.DatasetId == idDataset);
            var datasetAPIConfig = datasetAPIConfigs.FirstOrDefault();

            string? metadata = dataset.Metadata;
            if (string.IsNullOrWhiteSpace(metadata))
            {
                await Notify("Lỗi, dữ liệu chưa được đồng bộ!");
                throw new EntityNotFoundException(string.Format(_localizer["metadata.notfound"], idDataset));
            }

            List<MetadataDto>? listMetadatas = JsonConvert.DeserializeObject<List<MetadataDto>>(metadata);

            string? sql = $"CREATE TABLE [{dataset.TableName}-{idDataset}] ([DocId] uniqueidentifier NOT NULL";
            foreach (var metadataitem in listMetadatas)
            {
                if (string.Equals(metadataitem.DataType, "string", StringComparison.OrdinalIgnoreCase))
                {
                    sql += $",[{metadataitem.Data}] nvarchar(max) NULL";
                }
                else if (string.Equals(metadataitem.DataType, "datetime", StringComparison.OrdinalIgnoreCase))
                {
                    sql += $",[{metadataitem.Data}] datetime2 NULL";
                }
                else if (string.Equals(metadataitem.DataType, "boolean", StringComparison.OrdinalIgnoreCase))
                {
                    sql += $",[{metadataitem.Data}] bit NULL";
                }
                else if (string.Equals(metadataitem.DataType, "int", StringComparison.OrdinalIgnoreCase))
                {
                    sql += $",[{metadataitem.Data}] int NULL";
                }
                else if (string.Equals(metadataitem.DataType, "decimal", StringComparison.OrdinalIgnoreCase))
                {
                    sql += $",[{metadataitem.Data}] DECIMAL(10,4) NULL";
                }
            }

            sql += ");";

            // await _repository.ExecuteAsync(sql);

            var client = new RestClient();
            var request_ = new RestRequest(datasetAPIConfig.Url, string.Equals(datasetAPIConfig.Method, "post", StringComparison.OrdinalIgnoreCase) ? Method.Post :
                Method.Get);

            try
            {
                var headers = _serializerService.Deserialize<Dictionary<string, string>>(datasetAPIConfig.Headers);

                foreach (string? k in headers.Keys)
                {
                    string? value = headers[k];
                    request_.AddHeader(k, value);
                }
            }
            catch (Exception)
            {
            }

            if (!string.IsNullOrWhiteSpace(datasetAPIConfig.Data))
            {
                request_.AddParameter("application/json", datasetAPIConfig.Data, ParameterType.RequestBody);
            }

            var cancellationTokenSource = new CancellationTokenSource();
            var restResponse = await client.ExecuteAsync(request_, cancellationTokenSource.Token);

            string? content = restResponse.Content;
            dynamic contentObj = JObject.Parse(content);

            dynamic result = contentObj[datasetAPIConfig.DataKey];

            var count = result.Count;
            if (count < 1)
            {
                throw new Exception(string.Format(_localizer["dataset.notfound"], idDataset));
            }

            string? insertSql = $"INSERT INTO [{dataset.TableName}-{idDataset}] ([Docid],{string.Join(", ", listMetadatas.Select(e => $"[{e.Data}]"))}) VALUES  (@Docid, {string.Join(", ", listMetadatas.Select(e => $"@{e.Data}"))})";

            var sqlToExecute = new List<Tuple<string, DynamicParameters>>();

            foreach (dynamic itemResult in result)
            {
                var p = new DynamicParameters();
                p.Add("@Docid", Guid.NewGuid());
                p.Add("@id", "123");
                p.Add("@rank", "Nguyễn Tùng Lâm");
                p.Add("@title", "123");
                p.Add("@fullTitle", "Các doanh nghiệp có liên quan đến Tân Hoàng Minh vay trái phiếu khoảng 14.320 tỷ đồng trong cả năm 2021, dù trái phiếu bất động sản bị siết chặt.");
                p.Add("@year", "11111");
                p.Add("@image", "123");
                p.Add("@crew", "123");
                p.Add("@imDbRating", "123");
                p.Add("@imDbRatingCount", "123");

                try
                {
                    await _repository.ExecuteAsync(insertSql, p);
                }
                catch (Exception)
                {
                }
            }

            await _eventService.PublishAsync(new StatsChangedEvent());
            await Notify("Dữ liệu đồng bộ xong!");
        }
        catch (Exception ex)
        {
            await Notify("Dữ liệu đồng bộ lỗi!");
            throw new Exception(string.Format(_localizer["dataset.notfound"], ex.Message));
        }
    }

    public DataTable ToDataTable<T>(dynamic items)
    {
        DataTable dtDataTable = new DataTable();
        if (items.Count == 0) return dtDataTable;

        ((IEnumerable)items[0]).Cast<dynamic>().Select(p => p.Name).ToList().ForEach(col => { dtDataTable.Columns.Add(col); });

        ((IEnumerable)items).Cast<dynamic>().ToList().
            ForEach(data =>
            {
                DataRow dr = dtDataTable.NewRow();
                ((IEnumerable)data).Cast<dynamic>().ToList().ForEach(Col => { dr[Col.Name] = Col.Value; });
                dtDataTable.Rows.Add(dr);
            });
        return dtDataTable;
    }

    public DataTable ConvertToDataTable<T>(IList<T> data)
    {
        PropertyDescriptorCollection properties =
           TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }

        return table;
    }

    public Task DatasetDatabaseAsync(Guid idDataset)
    {
        throw new NotImplementedException();
    }

    public Task DatasetFileAsync(Guid idDataset)
    {
        throw new NotImplementedException();
    }
}