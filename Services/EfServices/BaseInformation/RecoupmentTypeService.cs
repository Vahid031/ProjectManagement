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
using ViewModels.BaseInformation.RecoupmentTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class RecoupmentTypeService : GenericService<RecoupmentType>, IRecoupmentTypeService
    {
        public RecoupmentTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListRecoupmentTypeViewModel> GetAll(ListRecoupmentTypeViewModel lctvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("RecoupmentType.", "");
            DynamicFiltering.GetFilter<ListRecoupmentTypeViewModel>(lctvm, ref pg);
            pg._filter = pg._filter.Replace("RecoupmentType.", "");

            var a = _uow.Set<RecoupmentType>().AsQueryable<RecoupmentType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListRecoupmentTypeViewModel()
            {
                RecoupmentType = new RecoupmentType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
