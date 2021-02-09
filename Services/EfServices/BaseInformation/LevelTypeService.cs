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
using ViewModels.BaseInformation.LevelTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class LevelTypeService : GenericService<LevelType>, ILevelTypeService
    {
        public LevelTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListLevelTypeViewModel> GetAll(ListLevelTypeViewModel lctvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("LevelType.", "");
            DynamicFiltering.GetFilter<ListLevelTypeViewModel>(lctvm, ref pg);
            pg._filter = pg._filter.Replace("LevelType.", "");

            var a = _uow.Set<LevelType>().AsQueryable<LevelType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListLevelTypeViewModel()
            {
                LevelType = new LevelType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
