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
using ViewModels.ProjectDefine.ProjectInvestmentChapterViewModel;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDefine
{
    public class ProjectInvestmentChapterService : GenericService<ProjectInvestmentChapter>, IProjectInvestmentChapterService
    {
        public ProjectInvestmentChapterService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectInvestmentChapterViewModel> GetAll(ListProjectInvestmentChapterViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectInvestmentChapterViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectInvestmentChapter>()
                .Join(_uow.Set<InvestmentChapterType>(), ProjectInvestmentChapter => ProjectInvestmentChapter.InvestmentChapterTypeId, InvestmentChapterType => InvestmentChapterType.Id, (ProjectInvestmentChapter, InvestmentChapterType) => new { ProjectInvestmentChapter, InvestmentChapterType })
                .Join(_uow.Set<InvestmentChapter>(), ProjectInvestmentChapter => ProjectInvestmentChapter.InvestmentChapterType.InvestmentChapterId, InvestmentChapter => InvestmentChapter.Id, (ProjectInvestmentChapter, InvestmentChapter) => new { ProjectInvestmentChapter.ProjectInvestmentChapter, ProjectInvestmentChapter.InvestmentChapterType, InvestmentChapter })
                .Join(_uow.Set<FinantialYear>(), ProjectInvestmentChapter => ProjectInvestmentChapter.ProjectInvestmentChapter.FinantialYearId, FinantialYear => FinantialYear.Id, (ProjectInvestmentChapter, FinantialYear) => new { ProjectInvestmentChapter.ProjectInvestmentChapter, ProjectInvestmentChapter.InvestmentChapterType, ProjectInvestmentChapter.InvestmentChapter, FinantialYear })
                ;

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectInvestmentChapterViewModel()
            {
                ProjectInvestmentChapter = new ProjectInvestmentChapter() { Id = Result.ProjectInvestmentChapter.Id, ProjectId = Result.ProjectInvestmentChapter.ProjectId, InvestmentChapterTypeId = Result.ProjectInvestmentChapter.InvestmentChapterTypeId, Price = Result.ProjectInvestmentChapter.Price, FinantialYearId = Result.ProjectInvestmentChapter.FinantialYearId },
                InvestmentChapter = new InvestmentChapter() { Id = Result.InvestmentChapter.Id, Title = Result.InvestmentChapter.Title },
                InvestmentChapterType = new InvestmentChapterType() { Id = Result.InvestmentChapterType.Id, Title = Result.InvestmentChapterType.Title, InvestmentChapterId = Result.InvestmentChapterType.InvestmentChapterId },
                FinantialYear = new FinantialYear() { Id = Result.FinantialYear.Id, Title = Result.FinantialYear.Title, IsActive = Result.FinantialYear.IsActive }
            });
        }
    }
}
