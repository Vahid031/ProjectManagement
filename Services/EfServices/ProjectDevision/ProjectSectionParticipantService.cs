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
using ViewModels.ProjectDevision.ProjectSectionParticipantViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionParticipantService : GenericService<ProjectSectionParticipant>, IProjectSectionParticipantService
    {
        public ProjectSectionParticipantService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionParticipantViewModel> GetAll(ListProjectSectionParticipantViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionParticipant.", "");
            DynamicFiltering.GetFilter<ListProjectSectionParticipantViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionParticipant.", "");

            var a = _uow.Set<ProjectSectionParticipant>().AsQueryable<ProjectSectionParticipant>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionParticipantViewModel()
            {
                ProjectSectionParticipant = new ProjectSectionParticipant() { Id = Result.Id, ParticipantName = Result.ParticipantName, ProjectSectionId = Result.ProjectSectionId }
            });
        }

    }
}
