using DatabaseContext.Context;
using DomainModels.Entities;
using DomainModels.Entities.General;
using Services.Interfaces;
using Services.Interfaces.General;

namespace Services.EfServices.General
{
    public class LogDetailService : GenericService<LogDetail>, ILogDetailService
    {
        public LogDetailService(IUnitOfWork uow)
            : base(uow)
        {
            
        }

    }
}

