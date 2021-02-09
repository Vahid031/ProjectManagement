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
using ViewModels.BaseInformation.CreditProvidePlaceViewModel;

namespace Services.EfServices.BaseInformation
{
    public class CreditProvidePlaceService : GenericService<CreditProvidePlace>, ICreditProvidePlaceService
    {
        public CreditProvidePlaceService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListCreditProvidePlaceViewModel> GetAll(ListCreditProvidePlaceViewModel lcppvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("CreditProvidePlace.", "");
            DynamicFiltering.GetFilter<ListCreditProvidePlaceViewModel>(lcppvm, ref pg);
            pg._filter = pg._filter.Replace("CreditProvidePlace.", "");

            var a = _uow.Set<CreditProvidePlace>().AsQueryable<CreditProvidePlace>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListCreditProvidePlaceViewModel()
            {
                CreditProvidePlace = new CreditProvidePlace() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
