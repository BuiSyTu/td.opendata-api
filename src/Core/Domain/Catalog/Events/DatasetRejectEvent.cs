using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Common.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog.Events;

public class DatasetRejectEvent : DomainEvent
{
    public DatasetRejectEvent(Dataset dataset)
    {
        Dataset = dataset;
        Dataset.ApproveState = 2;
    }

    public Dataset Dataset { get; }
}
