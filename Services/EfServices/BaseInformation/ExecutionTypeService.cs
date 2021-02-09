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
using ViewModels.BaseInformation.ExecutionTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ExecutionTypeService : GenericService<ExecutionType>, IExecutionTypeService
    {
        public ExecutionTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListExecutionTypeViewModel> GetAll(ListExecutionTypeViewModel letvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ExecutionType.", "");
            DynamicFiltering.GetFilter<ListExecutionTypeViewModel>(letvm, ref pg);
            pg._filter = pg._filter.Replace("ExecutionType.", "");

            var a = _uow.Set<ExecutionType>().AsQueryable<ExecutionType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListExecutionTypeViewModel()
            {
                ExecutionType = new ExecutionType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
