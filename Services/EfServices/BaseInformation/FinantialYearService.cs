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
using ViewModels.BaseInformation.FinantialYearViewModel;

namespace Services.EfServices.BaseInformation
{
    public class FinantialYearService : GenericService<FinantialYear>, IFinantialYearService
    {
        public FinantialYearService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListFinantialYearViewModel> GetAll(ListFinantialYearViewModel letvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("FinantialYear.", "");
            DynamicFiltering.GetFilter<ListFinantialYearViewModel>(letvm, ref pg);
            pg._filter = pg._filter.Replace("FinantialYear.", "");

            var a = _uow.Set<FinantialYear>().AsQueryable<FinantialYear>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListFinantialYearViewModel()
            {
                FinantialYear = new FinantialYear() { Id = Result.Id, Title = Result.Title, IsActive = Result.IsActive }
            });
        }
    }
}
