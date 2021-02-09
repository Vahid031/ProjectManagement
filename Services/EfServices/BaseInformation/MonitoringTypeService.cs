using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.BaseInformation;
using ViewModels.BaseInformation.MonitoringTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class MonitoringTypeService : GenericService<MonitoringType>, IMonitoringTypeService
    {
        public MonitoringTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }
        public IEnumerable<ListMonitoringTypeViewModel> GetAll(ListMonitoringTypeViewModel lmtvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("MonitoringType.", "");
            DynamicFiltering.GetFilter<ListMonitoringTypeViewModel>(lmtvm, ref pg);
            pg._filter = pg._filter.Replace("MonitoringType.", "");

            var a = _uow.Set<MonitoringType>().AsQueryable<MonitoringType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListMonitoringTypeViewModel()
            {
                MonitoringType = new MonitoringType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
