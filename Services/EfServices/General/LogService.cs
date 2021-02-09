using DatabaseContext.Context;
using DomainModels.Entities;
using DomainModels.Entities.General;
using Services.Interfaces;
using Services.Interfaces.General;

namespace Services.EfServices.General
{
    
    public class LogService : GenericService<Log>, ILogService
    {
        public LogService(IUnitOfWork uow)
            : base(uow)
        {
            
        }

    }
}