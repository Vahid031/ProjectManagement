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
using ViewModels.BaseInformation.StateViewModel;

namespace Services.EfServices.BaseInformation
{
    public class StateService : GenericService<State>, IStateService
    {
        public StateService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListStateViewModel> GetAll(ListStateViewModel lstvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("State.", "");
            DynamicFiltering.GetFilter<ListStateViewModel>(lstvm, ref pg);
            pg._filter = pg._filter.Replace("State.", "");

            var a = _uow.Set<State>().AsQueryable<State>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListStateViewModel()
            {
                State = new State() { Id = Result.Id, Title = Result.Title, Code = Result.Code }
            });
        }

    }
}
