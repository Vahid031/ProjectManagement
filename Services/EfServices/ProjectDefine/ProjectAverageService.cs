using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using DomainModels.Entities.ProjectDefine;
using Services.Interfaces.ProjectDefine;
using ViewModels.ProjectDefine.ProjectAverageViewModel;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDefine
{
    public class ProjectAverageService : GenericService<ProjectAverage>, IProjectAverageService
    {
        public ProjectAverageService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectAverageViewModel> GetAll(ListProjectAverageViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectAverageViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectAverage>()
                .Join(_uow.Set<FinantialYear>(), ProjectAverage => ProjectAverage.FinantialYearId, FinantialYear => FinantialYear.Id, (ProjectAverage, FinantialYear) => new { ProjectAverage, FinantialYear })
                ;

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectAverageViewModel()
            {
                ProjectAverage = new ProjectAverage() { Id = Result.ProjectAverage.Id, ProjectId = Result.ProjectAverage.ProjectId, FinantialYearId = Result.ProjectAverage.FinantialYearId, Average = Result.ProjectAverage.Average },
                FinantialYear = new FinantialYear() { Id = Result.FinantialYear.Id, Title = Result.FinantialYear.Title, IsActive = Result.FinantialYear.IsActive }
            });
        }
    }
}
