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
using ViewModels.BaseInformation.StatementTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class StatementTypeService : GenericService<StatementType>, IStatementTypeService
    {
        public StatementTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListStatementTypeViewModel> GetAll(ListStatementTypeViewModel lstvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("StatementType.", "");
            DynamicFiltering.GetFilter<ListStatementTypeViewModel>(lstvm, ref pg);
            pg._filter = pg._filter.Replace("StatementType.", "");

            var a = _uow.Set<StatementType>().AsQueryable<StatementType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListStatementTypeViewModel()
            {
                StatementType = new StatementType() { Id = Result.Id, Title = Result.Title }
            });
        }

    }
}
