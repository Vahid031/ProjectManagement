using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using ViewModels.ProjectDevision.ProjectSectionOperationTitleViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionOperationTitleService : GenericService<ProjectSectionOperationTitle>, IProjectSectionOperationTitleService
    {
        public ProjectSectionOperationTitleService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionOperationTitleViewModel> GetAll(ListProjectSectionOperationTitleViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionOperationTitle.", "");
            DynamicFiltering.GetFilter<ListProjectSectionOperationTitleViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionOperationTitle.", "");

            var a = _uow.Set<ProjectSectionOperationTitle>().AsQueryable<ProjectSectionOperationTitle>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionOperationTitleViewModel()
            {
                ProjectSectionOperationTitle = new ProjectSectionOperationTitle() { Id = Result.Id, Title = Result.Title, ProjectSectionId = Result.ProjectSectionId }
            });
        }

    }
}
