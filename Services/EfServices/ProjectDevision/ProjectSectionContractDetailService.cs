using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionContractDetailService : GenericService<ProjectSectionContractDetail>, IProjectSectionContractDetailService
    {
        public ProjectSectionContractDetailService(IUnitOfWork uow)
            : base(uow)
        {

        }

    }
}
