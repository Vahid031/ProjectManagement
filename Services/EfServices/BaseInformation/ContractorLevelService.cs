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
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;

namespace Services.EfServices.BaseInformation
{
    public class ContractorLevelService : GenericService<ContractorLevel>, IContractorLevelService
    {
        public ContractorLevelService(IUnitOfWork uow)
            : base(uow)
        {

        }

    }
}
