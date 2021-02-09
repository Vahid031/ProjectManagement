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
using ViewModels.BaseInformation.InvestmentChapterViewModel;

namespace Services.EfServices.BaseInformation
{
    public class InvestmentChapterService : GenericService<InvestmentChapter>, IInvestmentChapterService
    {
        public InvestmentChapterService(IUnitOfWork uow)
            : base(uow)
        {

        }


        public IEnumerable<ListInvestmentChapterViewModel> GetAll(ListInvestmentChapterViewModel licvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("InvestmentChapter.", "");
            DynamicFiltering.GetFilter<ListInvestmentChapterViewModel>(licvm, ref pg);
            pg._filter = pg._filter.Replace("InvestmentChapter.", "");

            var a = _uow.Set<InvestmentChapter>().AsQueryable<InvestmentChapter>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListInvestmentChapterViewModel()
            {
                InvestmentChapter = new InvestmentChapter() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
