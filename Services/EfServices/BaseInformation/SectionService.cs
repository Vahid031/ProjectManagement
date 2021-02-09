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
using ViewModels.BaseInformation.SectionViewModel;

namespace Services.EfServices.BaseInformation
{
    public class SectionService : GenericService<Section>, ISectionService
    {
        public SectionService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListSectionViewModel> GetAll(ListSectionViewModel lrdvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListSectionViewModel>(lrdvm, ref pg);

            var a =
            _uow.Set<Section>()
                .Join(_uow.Set<City>(), Section => Section.CityId, City => City.Id, (Section, City) => new { Section, City })
                .Join(_uow.Set<State>(), Section => Section.City.StateId, State => State.Id, (Section, State) => new { Section.Section, Section.City, State });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListSectionViewModel()
            {
                Section = new Section() { Id = Result.Section.Id, CityId = Result.Section.CityId, Title = Result.Section.Title, Code = Result.Section.Code },
                City = new City() { Id = Result.City.Id, StateId = Result.City.StateId, Title = Result.City.Title, Code = Result.City.Code },
                State = new State() { Id = Result.State.Id, Title = Result.State.Title, Code = Result.State.Code }
            });
        }
    }
}
