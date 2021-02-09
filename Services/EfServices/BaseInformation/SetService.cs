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
using ViewModels.BaseInformation.SetViewModel;

namespace Services.EfServices.BaseInformation
{
    public class SetService : GenericService<Set>, ISetService
    {
        public SetService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListSetViewModel> GetAll(ListSetViewModel lrdvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("Set.", "");
            DynamicFiltering.GetFilter<ListSetViewModel>(lrdvm, ref pg);
            pg._filter = pg._filter.Replace("Set.", "");

            var a = _uow.Set<Set>().AsQueryable<Set>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListSetViewModel()
            {
                Set = new Set() { Id = Result.Id, Title = Result.Title, Code = Result.Code }
            });
        }
    }
}
