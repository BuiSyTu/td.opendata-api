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
using Hangfire;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using TD.OpenData.WebApi.Shared.DTOs.Dashboard;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class DatasetService : IDatasetService
{
    private readonly IStringLocalizer<DatasetService> _localizer;
    private readonly IRepositoryAsync _repository;
    private readonly IForwardService _forwardService;
    private readonly ISqlService _sqlService;
    private readonly IExcelReader _excelReader;
    private readonly ApplicationDbContext _dbContext;
    private readonly IDatasetAPIConfigService _datasetAPIConfigService;
    private readonly IDatasetFileConfigService _datasetFileConfigService;
    private readonly IDatasetDBConfigService _datasetDBConfigService;

    public DatasetService(
        IRepositoryAsync repository,
        IStringLocalizer<DatasetService> localizer,
        IForwardService forwardService,
        ISqlService sqlService,
        IExcelReader excelReader,
        ApplicationDbContext dbContext,
        IDatasetAPIConfigService datasetAPIConfigService,
        IDatasetFileConfigService datasetFileConfigService,
        IDatasetDBConfigService datasetDBConfigService)
    {
        _repository = repository;
        _localizer = localizer;
        _forwardService = forwardService;
        _sqlService = sqlService;
        _excelReader = excelReader;
        _dbContext = dbContext;
        _datasetAPIConfigService = datasetAPIConfigService;
        _datasetFileConfigService = datasetFileConfigService;
        _datasetDBConfigService = datasetDBConfigService;
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

    public Task<int> GetCountAsync(DatasetListFilter filter)
    {
        var filters = new Filters<Dataset>();
        filters.Add(filter.CategoryId.HasValue, x => x.CategoryId.Equals(filter.CategoryId!.Value));
        filters.Add(filter.LicenseId.HasValue, x => x.LicenseId.Equals(filter.LicenseId!.Value));
        filters.Add(filter.OrganizationId.HasValue, x => x.OrganizationId.Equals(filter.OrganizationId!.Value));
        filters.Add(filter.DataTypeId.HasValue, x => x.DataTypeId.Equals(filter.DataTypeId!.Value));
        filters.Add(filter.ProviderTypeId.HasValue, x => x.ProviderTypeId.Equals(filter.ProviderTypeId!.Value));
        filters.Add(filter.ApproveState.HasValue, x => x.ApproveState == filter.ApproveState!.Value);
        filters.Add(filter.Visibility.HasValue, x => x.Visibility == filter.Visibility!.Value);
        filters.Add(!string.IsNullOrEmpty(filter.DataTypeCode), x => x.DataType.Code == filter.DataTypeCode);

        return _repository.GetCountAsync(filters);
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
            var itemConfig = request.DatasetFileConfig.Adapt<DatasetFileConfig>();
            itemConfig.DatasetId = itemDatasetId;
            itemConfig.DomainEvents.Add(new StatsChangedEvent());
            await _repository.CreateAsync(itemConfig);
        }
        else if (string.Equals(dataType.Code, "database", StringComparison.OrdinalIgnoreCase) && request.DatasetDBConfig != null)
        {
            var itemConfig = request.DatasetDBConfig.Adapt<DatasetDBConfig>();
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

        BackgroundJob.Enqueue(() => _sqlService.DeleteTableSqlAsync(itemToDelete.TableName));

        itemToDelete.DomainEvents.Add(new StatsChangedEvent());
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public async Task<object> GetDataAsync(Guid id, string? orderBy, int skip, int top)
    {
        if (top > 1000) top = 1000;
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
        var item = await _repository.GetByIdAsync(id, includes);

        if (item != null)
        {
            if (item.View.HasValue)
            {
                item.View++;
            }
            else
            {
                item.View = 1;
            }

            await _repository.UpdateAsync(item);
            await _repository.SaveChangesAsync();
        }

        return await Result<DatasetDetailsDto>.SuccessAsync(item.Adapt<DatasetDetailsDto>());
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
        filters.Add(filter.Visibility.HasValue, x => x.Visibility == filter.Visibility.Value);
        filters.Add(!string.IsNullOrEmpty(filter.DataTypeCode), x => x.DataType.Code == filter.DataTypeCode);
        filters.Add(!string.IsNullOrEmpty(filter.Author), x => x.Author == filter.Author);
        filters.Add(!string.IsNullOrEmpty(filter.OfficeCode), x => x.OfficeCode == filter.OfficeCode);
        filters.Add(filter.IsPortal.HasValue && filter.IsPortal.Value, x => string.IsNullOrEmpty(x.OfficeCode));
        filters.Add(filter.IsOffice.HasValue && filter.IsOffice.Value, x => !string.IsNullOrEmpty(x.OfficeCode));

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
        bool isSynced = false;

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

            // Change state isSynced
            dataset.IsSynced = true;
            await _repository.SaveChangesAsync();
            isSynced = true;
        }

        var excelExtensions = new List<string> { ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm", ".xls", "xlt", "xlam", "xla", "xlw", "xlr" };
        bool isExcel = dataTypeCode == "file"
            && excelExtensions.Contains(Path.GetExtension(dataset?.DatasetFileConfig?.FileName));
        if (isExcel)
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

            // Change state isSynced
            dataset.IsSynced = true;
            await _repository.SaveChangesAsync();
            isSynced = true;
        }

        if (isSynced)
        {
            SyncHistory syncHistory = new()
            {
                DatasetId = id,
                SyncTime = DateTime.Now
            };

            _dbContext.SyncHistories.Add(syncHistory);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateDatasetRequest request, Guid id)
    {
        var item = await _repository.GetByIdAsync<Dataset>(id);
        if (item == null) throw new EntityNotFoundException(string.Format(_localizer["dataset.notfound"], id));

        var dataType = await _repository.GetByIdAsync<DataType>((Guid)request.DataTypeId);

        if (string.Equals(dataType.Code, "webapi", StringComparison.OrdinalIgnoreCase) && request.DatasetAPIConfig != null)
        {
            var itemConfig = _datasetAPIConfigService.GetByDatasetId(id);

            if (itemConfig == null)
            {
                itemConfig = request.DatasetAPIConfig.Adapt<DatasetAPIConfig>();
                itemConfig.DatasetId = id;
                itemConfig.DomainEvents.Add(new StatsChangedEvent());
                await _repository.CreateAsync(itemConfig);
            }
            else
            {
                var itemConfigToUpdate = itemConfig.Update(request.DatasetAPIConfig);
                itemConfigToUpdate.DomainEvents.Add(new StatsChangedEvent());
                await _repository.UpdateAsync(itemConfigToUpdate);
            }
        }
        else if (string.Equals(dataType.Code, "file", StringComparison.OrdinalIgnoreCase) && request.DatasetFileConfig != null)
        {
            var itemConfig = _datasetFileConfigService.GetByDatasetId(id);

            if (itemConfig == null)
            {
                itemConfig = request.DatasetFileConfig.Adapt<DatasetFileConfig>();
                itemConfig.DatasetId = id;
                itemConfig.DomainEvents.Add(new StatsChangedEvent());
                await _repository.CreateAsync(itemConfig);
            }
            else
            {
                var itemConfigToUpdate = itemConfig.Update(request.DatasetFileConfig);
                itemConfigToUpdate.DomainEvents.Add(new StatsChangedEvent());
                await _repository.UpdateAsync(itemConfigToUpdate);
            }
        }
        else if (string.Equals(dataType.Code, "database", StringComparison.OrdinalIgnoreCase) && request.DatasetDBConfig != null)
        {
            var itemConfig = _datasetDBConfigService.GetByDatasetId(id);

            if (itemConfig == null)
            {
                itemConfig = request.DatasetDBConfig.Adapt<DatasetDBConfig>();
                itemConfig.DatasetId = id;
                itemConfig.DomainEvents.Add(new StatsChangedEvent());
                await _repository.CreateAsync(itemConfig);
            }
            else
            {
                var itemConfigToUpdate = itemConfig.Update(request.DatasetDBConfig);
                itemConfigToUpdate.DomainEvents.Add(new StatsChangedEvent());
                await _repository.UpdateAsync(itemConfigToUpdate);
            }

        }

        var itemToUpdate = item.Update(request);
        itemToUpdate.DomainEvents.Add(new StatsChangedEvent());
        await _repository.UpdateAsync(itemToUpdate);
        await _repository.SaveChangesAsync();
        return await Result<Guid>.SuccessAsync(id);
    }

    public object GroupByCategory()
    {
        var result = _dbContext.Categories
            .Select(x => new DashboardItem
            {
                Name = x.Name,
                Count = 0
            })
            .ToList();

        var datasets = _dbContext.Datasets
            .GroupBy(x => x.Category.Name)
            .Select(g => new DashboardItem
            {
                Name = g.Key,
                Count = g.Count()
            })
            .ToList();

        datasets.ForEach(dataset =>
        {
            var category = result.FirstOrDefault(x => x.Name == dataset.Name);

            if (category != null)
            {
                category.Count = dataset.Count;
            }
        });

        return result;
    }

    public object GroupByOrganization()
    {
        var result = _dbContext.Organizations
            .Select(x => new DashboardItem
            {
                Name = x.Name,
                Count = 0
            })
            .ToList();

        var datasets = _dbContext.Datasets
            .GroupBy(x => x.Organization.Name)
            .Select(g => new
            {
                Name = g.Key,
                Count = g.Count()
            })
            .ToList();

        datasets.ForEach(dataset =>
        {
            var organization = result.FirstOrDefault(x => x.Name == dataset.Name);

            if (organization != null)
            {
                organization.Count = dataset.Count;
            }
        });

        return result;
    }
}