using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Identity.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Dashboard;
using Microsoft.Extensions.Localization;
using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;
using TD.OpenData.WebApi.Application.Dashboard;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class StatsService : IStatsService
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IRepositoryAsync _repository;
    private readonly IStringLocalizer<StatsService> _localizer;
    private readonly IDatasetService _datasetService;
    private readonly ApplicationDbContext _dbContext;

    public StatsService(
        IRepositoryAsync repository,
        IRoleService roleService,
        IUserService userService,
        IStringLocalizer<StatsService> localizer,
        IDatasetService datasetService,
        ApplicationDbContext dbContext)
    {
        _repository = repository;
        _roleService = roleService;
        _userService = userService;
        _localizer = localizer;
        _datasetService = datasetService;
        _dbContext = dbContext;
    }

    public async Task<IResult<StatsDto>> GetDataAsync()
    {
        var stats = new StatsDto
        {
            ProductCount = await _repository.GetCountAsync<Product>(),
            BrandCount = await _repository.GetCountAsync<Brand>(),
            UserCount = await _userService.GetCountAsync(),
            RoleCount = await _roleService.GetCountAsync()
        };

        int selectedYear = DateTime.Now.Year;
        double[] productsFigure = new double[13];
        double[] brandsFigure = new double[13];
        for (int i = 1; i <= 12; i++)
        {
            int month = i;
            var filterStartDate = new DateTime(selectedYear, month, 01);
            var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

            productsFigure[i - 1] = await _repository.GetCountAsync<Product>(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate);
            brandsFigure[i - 1] = await _repository.GetCountAsync<Brand>(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate);
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Products"], Data = productsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Brands"], Data = brandsFigure });

        return await Result<StatsDto>.SuccessAsync(stats);
    }

    public async Task<IResult<OverViewWidget>> GetOverViewAsync()
    {
        var widget = new OverViewWidget
        {
            Category = await _repository.GetCountAsync<Category>(),
            Dataset = await _repository.GetCountAsync<Dataset>(),
            Organization = await _repository.GetCountAsync<Organization>(),
            ProviderType = await _repository.GetCountAsync<ProviderType>(),
            DataType = await _repository.GetCountAsync<DataType>(),
            View = _dbContext.Datasets.Sum(x => x.View.HasValue ? x.View.Value : 0)
        };

        return await Result<OverViewWidget>.SuccessAsync(widget);
    }

    public async Task<IResult<Widget>> GetWidgetsAsync()
    {
        var widget = new Widget
        {
            WebAPI = await _datasetService.GetCountAsync(new DatasetListFilter
            {
                DataTypeCode = "webapi"
            }),
            Excel = await _datasetService.GetCountAsync(new DatasetListFilter
            {
                DataTypeCode = "excel"
            }),
            Database = await _datasetService.GetCountAsync(new DatasetListFilter
            {
                DataTypeCode = "database"
            })
        };

        return await Result<Widget>.SuccessAsync(widget);
    }
}