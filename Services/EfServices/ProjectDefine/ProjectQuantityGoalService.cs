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
using ViewModels.ProjectDefine.ProjectQuantityGoalViewModel;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDefine
{
    public class ProjectQuantityGoalService : GenericService<ProjectQuantityGoal>, IProjectQuantityGoalService
    {
        public ProjectQuantityGoalService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectQuantityGoalViewModel> GetAll(ListProjectQuantityGoalViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectQuantityGoalViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectQuantityGoal>()
                .Join(_uow.Set<Unit>(), ProjectQuantityGoal => ProjectQuantityGoal.UnitId, Unit => Unit.Id, (ProjectQuantityGoal, Unit) => new { ProjectQuantityGoal, Unit })
                ;

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectQuantityGoalViewModel()
            {
                ProjectQuantityGoal = new ProjectQuantityGoal() { Id = Result.ProjectQuantityGoal.Id, ProjectId = Result.ProjectQuantityGoal.ProjectId, Amount = Result.ProjectQuantityGoal.Amount, QuantityGoalType = Result.ProjectQuantityGoal.QuantityGoalType, UnitId = Result.ProjectQuantityGoal.UnitId, UnitPrice = Result.ProjectQuantityGoal.UnitPrice },
                Unit = new Unit() { Id = Result.Unit.Id, Title = Result.Unit.Title },
            });
        }
    }
}
