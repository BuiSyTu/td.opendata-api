using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DocumentType;

namespace TD.OpenData.WebApi.Application.AdministrativeCategories.Interfaces;

public interface IDocumentTypeService : ITransientService
{
    Task<Result<DocumentTypeDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<DocumentTypeDto>> SearchAsync(DocumentTypeListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateDocumentTypeRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateDocumentTypeRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}
