using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Common.Specifications;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.Extensions.Localization;
using Mapster;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using System.Linq.Expressions;
using TD.OpenData.WebApi.Application.Forward;
using TD.OpenData.WebApi.Domain.Catalog.Events;
using TD.OpenData.WebApi.Application.SyncData.Interfaces;
using TD.OpenData.WebApi.Infrastructure.SyncData;
using TD.OpenData.WebApi.Shared.DTOs.Filters;
using Hangfire;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class DatasetService : IDatasetService
{
    private readonly IStringLocalizer<DatasetService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IForwardService _forwardService;
    private readonly ISqlService _sqlService;
    private readonly IExcelReader _excelReader;

    public DatasetService(
        IRepositoryAsync repository,
        IStringLocalizer<DatasetService> localizer,
        IForwardService forwardService,
        ISqlService sqlService,
        IExcelReader excelReader)
    {
        _repository = repository;
        _localizer = localizer;
        _forwardService = forwardService;
        _sqlService = sqlService;
        _excelReader = excelReader;
    }

    public async Task<Result<Guid>> ApprovedAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<Dataset>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["dataset.notfound"], id));

        item.DomainEvents.Add(new DatasetApproveEvent(item));
        await _repository.UpdateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<Result<Guid>> CreateAsync(CreateDatasetRequest request)
    {
        bool itemExists = await _repository.ExistsAsync<Dataset>(a => a.Name == request.Name && a.Code == request.Code);
        if (itemExists) throw new EntityAlreadyExistsException(string.Format(_localizer["dataset.alreadyexists"], request.Name));

        bool licenseExists = await _repository.ExistsAsync<License>(a => a.Id == request.LicenseId);
        if (!licenseExists) throw new EntityNotFoundException(string.Format(_localizer["License.notfound"], request.LicenseId));

        bool organizationExists = await _repository.ExistsAsync<Organization>(a => a.Id == request.OrganizationId);
        if (!organizationExists) throw new EntityNotFoundException(string.Format(_localizer["Organization.notfound"], request.OrganizationId));

        bool dataTypeExists = await _repository.ExistsAsync<DataType>(a => a.Id == request.DataTypeId);
        if (!dataTypeExists) throw new EntityNotFoundException(string.Format(_localizer["DataType.notfound"], request.DataTypeId));

        bool categoryExists = await _repository.ExistsAsync<Category>(a => a.Id == request.CategoryId);
        if (!categoryExists) throw new EntityNotFoundException(string.Format(_localizer["Category.notfound"], request.CategoryId));

        bool providerTypeExists = await _repository.ExistsAsync<ProviderType>(a => a.Id == request.ProviderTypeId);
        if (!providerTypeExists) throw new EntityNotFoundException(string.Format(_localizer["ProviderType.notfound"], request.ProviderTypeId));

        var item = request.Adapt<Dataset>();
        item.DomainEvents.Add(new StatsChangedEvent());
        var itemDatasetId = await _repository.CreateAsync(item);

        var dataType = await _repository.GetByIdAsync<DataType>((Guid)request.DataTypeId);

        if (string.Equals(dataType.Code, "webapi", StringComparison.OrdinalIgnoreCase) && request.DatasetAPIConfig != null)
        {
            var itemConfig = request.DatasetAPIConfig.Adapt<DatasetAPIConfig>();
            itemConfig.DatasetId = itemDatasetId;
            itemConfig.DomainEvents.Add(new StatsChangedEvent());
            await _repository.CreateAsync(itemConfig);
        }
        else if (string.Equals(dataType.Code, "file", StringComparison.OrdinalIgnoreCase) && request.DatasetFileConfig != null)
        {
            var itemConfig = request.DatasetAPIConfig.Adapt<DatasetFileConfig>();
            itemConfig.DatasetId = itemDatasetId;
            itemConfig.DomainEvents.Add(new StatsChangedEvent());
            await _repository.CreateAsync(itemConfig);
        }
        else if (string.Equals(dataType.Code, "excel", StringComparison.OrdinalIgnoreCase) && request.DatasetFileConfig != null)
        {
            var itemConfig = request.DatasetAPIConfig.Adapt<DatasetFileConfig>();
            itemConfig.DatasetId = itemDatasetId;
            itemConfig.DomainEvents.Add(new StatsChangedEvent());
            await _repository.CreateAsync(itemConfig);
        }
        else if (string.Equals(dataType.Code, "database", StringComparison.OrdinalIgnoreCase) && request.DatasetDBConfig != null)
        {
            var itemConfig = request.DatasetAPIConfig.Adapt<DatasetDBConfig>();
            itemConfig.DatasetId = itemDatasetId;
            itemConfig.DomainEvents.Add(new StatsChangedEvent());
            await _repository.CreateAsync(itemConfig);
        }

        await _repository.SaveChangesAsync();

        string? jobId = BackgroundJob.Enqueue(() => SyncDataAsync(itemDatasetId));

        return await Result<Guid>.SuccessAsync(itemDatasetId);
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id)
    {
        var itemToDelete = await _repository.RemoveByIdAsync<Dataset>(id);
        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<object> GetDataAsync(Guid id, string? orderBy, int skip, int top)
    {
        var dataset = await _repository.GetByIdAsync<Dataset>(id);

        string sql =
        $@"SELECT * FROM {dataset.TableName}
            ORDER BY {(string.IsNullOrEmpty(orderBy) ? "ID" : orderBy)}
            OFFSET {skip} ROWS FETCH NEXT {top} ROWS ONLY
        ";

        var result = await _repository.QueryAsync<object>(sql);
        return result;
    }

    public async Task<Result<DatasetDetailsDto>> GetDetailsAsync(Guid id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var includes = new Expression<Func<Dataset, object>>[]
        {
            x => x.Category, y => y.DataType, z => z.Organization,
            x => x.License, x => x.ProviderType, x => x.DatasetAPIConfig,
            x => x.DatasetDBConfig, x => x.DatasetFileConfig
        };
#pragma warning restore CS8603 // Possible null reference return.
        var item = await _repository.GetByIdAsync<Dataset, DatasetDetailsDto>(id, includes);
        return await Result<DatasetDetailsDto>.SuccessAsync(item);
    }

    public async Task<Result<Guid>> RejectedAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync<Dataset>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["dataset.notfound"], id));

        item.DomainEvents.Add(new DatasetRejectEvent(item));
        await _repository.UpdateAsync(item);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<PaginatedResult<DatasetDto>> SearchAsync(DatasetListFilter filter)
    {
        var filters = new Filters<Dataset>();
        filters.Add(filter.CategoryId.HasValue, x => x.CategoryId.Equals(filter.CategoryId!.Value));
        filters.Add(filter.LicenseId.HasValue, x => x.LicenseId.Equals(filter.LicenseId!.Value));
        filters.Add(filter.OrganizationId.HasValue, x => x.OrganizationId.Equals(filter.OrganizationId!.Value));
        filters.Add(filter.DataTypeId.HasValue, x => x.DataTypeId.Equals(filter.DataTypeId!.Value));
        filters.Add(filter.ProviderTypeId.HasValue, x => x.ProviderTypeId.Equals(filter.ProviderTypeId!.Value));
        filters.Add(filter.ApproveState.HasValue, x => x.ApproveState == filter.ApproveState!.Value);
        filters.Add(filter.Visibility.HasValue, x => x.Visibility == filter.Visibility!.Value);

        var specification = new PaginationSpecification<Dataset>
        {
            AdvancedSearch = filter.AdvancedSearch,
            Filters = filters,
            Keyword = filter.Keyword,
            OrderBy = x => x.OrderBy(b => b.Name),
            OrderByStrings = filter.OrderBy,
            PageIndex = filter.PageNumber,
            PageSize = filter.PageSize,
            Includes = new Expression<Func<Dataset, object>>[] { x => x.Category, z => z.Organization }
        };

        return await _repository.GetListAsync<Dataset, DatasetDto>(specification);
    }

    public async Task SyncDataAsync(Guid id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var includes = new Expression<Func<Dataset, object>>[]
        {
            x => x.DatasetAPIConfig, x => x.DatasetDBConfig, x => x.DatasetFileConfig, x => x.DataType
        };
#pragma warning restore CS8603 // Possible null reference return.
        Dataset? dataset = await _repository.GetByIdAsync<Dataset>(id, includes);
        string? dataTypeCode = dataset?.DataType?.Code?.ToLower();

        if (dataTypeCode == "webapi")
        {
            // Create table
            string? dataText = await _forwardService.ForwardDataset(dataset);
            var previewData = dataText.ToPreviewData(dataset.DatasetAPIConfig.DataKey);
            string? tableName = await _sqlService.CreateTableSqlAsync(previewData.Metadata);

            // Save table name to dataset
            dataset.TableName = tableName;
            await _repository.SaveChangesAsync();

            // Import data
            await _sqlService.ImportDataAsync(tableName, previewData.Metadata, previewData.Data);
        }

        if (dataTypeCode == "excel")
        {
            byte[] fileBytes = await File.ReadAllBytesAsync(dataset.DatasetFileConfig.FileUrl);
            string? dataText = _excelReader.GetJsonData(new MemoryStream(fileBytes), dataset.DatasetFileConfig.SheetName);

            var previewData = dataText.ToPreviewData(dataset.DatasetFileConfig.SheetName);
            string? tableName = await _sqlService.CreateTableSqlAsync(previewData.Metadata);

            // Save table name to dataset
            dataset.TableName = tableName;
            await _repository.SaveChangesAsync();

            // Import data
            await _sqlService.ImportDataAsync(tableName, previewData.Metadata, previewData.Data);
        }
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateDatasetRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<Dataset>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["dataset.notfound"], id));

        var itemToUpdate = item.Update(request);
        itemToUpdate.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(itemToUpdate);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }
}