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
using ViewModels.BaseInformation.UnitViewModel;

namespace Services.EfServices.BaseInformation
{
    public class UnitService : GenericService<Unit>, IUnitService
    {
        public UnitService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListUnitViewModel> GetAll(ListUnitViewModel lrdvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("Unit.", "");
            DynamicFiltering.GetFilter<ListUnitViewModel>(lrdvm, ref pg);
            pg._filter = pg._filter.Replace("Unit.", "");

            var a = _uow.Set<Unit>().AsQueryable<Unit>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListUnitViewModel()
            {
                Unit = new Unit() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
