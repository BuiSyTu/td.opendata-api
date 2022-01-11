﻿using TD.OpenData.WebApi.Application.Catalog.Interfaces;
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

        var dataset = await _repository.GetByIdAsync<Dataset>(idDataset);
        if (dataset == null)
        {
            await Notify("Lỗi, dữ liệu chưa được đồng bộ!");
            throw new EntityNotFoundException(string.Format(_localizer["dataset.notfound"], idDataset));
        }

        var datasetAPIConfigs = await _repository.GetListAsync<DatasetAPIConfig>(x => x.DatasetId == idDataset);
        var datasetAPIConfig = datasetAPIConfigs.FirstOrDefault();


        var metadata = dataset.Metadata;
        if (string.IsNullOrWhiteSpace(metadata))
        {
            await Notify("Lỗi, dữ liệu chưa được đồng bộ!");
            throw new EntityNotFoundException(string.Format(_localizer["metadata.notfound"], idDataset));
        }

        var client = new RestClient();
        var request_ = new RestRequest(datasetAPIConfig.Url, string.Equals(datasetAPIConfig.Method, "post", StringComparison.OrdinalIgnoreCase) ? Method.Post :
            Method.Get);

        var headers = _serializerService.Deserialize<Dictionary<string, string>>(datasetAPIConfig.Headers);

        var keys = headers.Keys;
        foreach (var k in keys)
        {
            var value = headers[k];
            request_.AddHeader(k, value);
        }

        if (!string.IsNullOrWhiteSpace(datasetAPIConfig.Data))
        {
            request_.AddParameter("application/json", datasetAPIConfig.Data, ParameterType.RequestBody);
        }

        var cancellationTokenSource = new CancellationTokenSource();
        var restResponse = await client.ExecuteAsync(request_, cancellationTokenSource.Token);

        var content = restResponse.Content;
        dynamic contentObj = JObject.Parse(content);
        dynamic result = contentObj[datasetAPIConfig.DataKey];




        //var product = await _repository.QueryFirstOrDefaultAsync<Object>($"SELECT * FROM public.\"Products\" WHERE \"Id\"  = '{id}' AND \"Tenant\"='@tenant'");


        var sql = $"CREATE TABLE [{idDataset}] ([DocId] uniqueidentifier NOT NULL,[Name] nvarchar(max) NOT NULL);";

        var product = await _repository.ExecuteAsync(sql);




        await _eventService.PublishAsync(new StatsChangedEvent());
        await Notify("Dữ liệu đồng bộ xong!");

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