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
using ViewModels.BaseInformation.ResourceViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ResourceService : GenericService<Resource>, IResourceService
    {
        public ResourceService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListResourceViewModel> GetAll(ListResourceViewModel lptvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("Resource.", "");
            DynamicFiltering.GetFilter<ListResourceViewModel>(lptvm, ref pg);
            pg._filter = pg._filter.Replace("Resource.", "");

            var a = _uow.Set<Resource>().AsQueryable<Resource>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListResourceViewModel()
            {
                Resource = new Resource() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
