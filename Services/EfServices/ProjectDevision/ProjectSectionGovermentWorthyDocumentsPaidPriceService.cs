using System;
using System.Collections.Generic;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using DatabaseContext.Context;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionGovermentWorthyDocumentsPaidPriceService : GenericService<ProjectSectionGovermentWorthyDocumentsPaidPrice>, IProjectSectionGovermentWorthyDocumentsPaidPriceService
    {
        public ProjectSectionGovermentWorthyDocumentsPaidPriceService(IUnitOfWork uow)
            : base(uow)
        {

        }

    }
}
