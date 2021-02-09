
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
using ViewModels.BaseInformation.InvestmentChapterTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class InvestmentChapterTypeService : GenericService<InvestmentChapterType>, IInvestmentChapterTypeService
    {
        public InvestmentChapterTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListInvestmentChapterTypeViewModel> GetAll(ListInvestmentChapterTypeViewModel licvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("InvestmentChapterType.", "");
            DynamicFiltering.GetFilter<ListInvestmentChapterTypeViewModel>(licvm, ref pg);
            pg._filter = pg._filter.Replace("InvestmentChapterType.", "");

            var a = _uow.Set<InvestmentChapterType>().AsQueryable<InvestmentChapterType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListInvestmentChapterTypeViewModel()
            {
                InvestmentChapterType = new InvestmentChapterType() { Id = Result.Id, Title = Result.Title, InvestmentChapterId = Result.InvestmentChapterId }
            });
        }
    }
}
