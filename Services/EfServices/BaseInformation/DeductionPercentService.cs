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
using ViewModels.BaseInformation.DeductionPercentViewModel;

namespace Services.EfServices.BaseInformation
{
    public class DeductionPercentService : GenericService<DeductionPercent>, IDeductionPercentService
    {
        public DeductionPercentService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListDeductionPercentViewModel> GetAll(ListDeductionPercentViewModel ldpvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("DeductionPercent.", "");
            DynamicFiltering.GetFilter<ListDeductionPercentViewModel>(ldpvm, ref pg);
            pg._filter = pg._filter.Replace("DeductionPercent.", "");

            var a = _uow.Set<DeductionPercent>().AsQueryable<DeductionPercent>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListDeductionPercentViewModel()
            {
                DeductionPercent = new DeductionPercent() { Id = Result.Id, Title = Result.Title, PercentAmount = Result.PercentAmount }
            });
        }
    }
}
