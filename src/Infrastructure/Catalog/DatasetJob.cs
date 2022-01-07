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

namespace TD.OpenData.WebApi.Infrastructure.Catalog;

public class DatasetJob : IBrandGeneratorJob
{
    private readonly IEventService _eventService;
    private readonly ILogger<DatasetJob> _logger;
    private readonly IRepositoryAsync _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationService _notificationService;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public DatasetJob(
        ILogger<DatasetJob> logger,
        IRepositoryAsync repository,
        IProgressBarFactory progressBar,
        PerformingContext performingContext,
        INotificationService notificationService,
        ICurrentUser currentUser,
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
}