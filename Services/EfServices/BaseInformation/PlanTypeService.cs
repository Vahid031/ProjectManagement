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
using ViewModels.BaseInformation.PlanTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class PlanTypeService : GenericService<PlanType>, IPlanTypeService
    {
        public PlanTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListPlanTypeViewModel> GetAll(ListPlanTypeViewModel lctvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("PlanType.", "");
            DynamicFiltering.GetFilter<ListPlanTypeViewModel>(lctvm, ref pg);
            pg._filter = pg._filter.Replace("PlanType.", "");

            var a = _uow.Set<PlanType>().AsQueryable<PlanType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListPlanTypeViewModel()
            {
                PlanType = new PlanType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
