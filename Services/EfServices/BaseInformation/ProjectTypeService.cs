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
using ViewModels.BaseInformation.ProjectTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ProjectTypeService : GenericService<ProjectType>, IProjectTypeService
    {
        public ProjectTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectTypeViewModel> GetAll(ListProjectTypeViewModel lptvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectType.", "");
            DynamicFiltering.GetFilter<ListProjectTypeViewModel>(lptvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectType.", "");

            var a = _uow.Set<ProjectType>().AsQueryable<ProjectType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectTypeViewModel()
            {
                ProjectType = new ProjectType() { Id = Result.Id, Title = Result.Title }
            });
        }

    }
}
