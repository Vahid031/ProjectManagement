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
using ViewModels.BaseInformation.VillageViewModel;

namespace Services.EfServices.BaseInformation
{
    public class VillageService : GenericService<Village>, IVillageService
    {
        public VillageService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListVillageViewModel> GetAll(ListVillageViewModel lvvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListVillageViewModel>(lvvm, ref pg);

            var a =
            _uow.Set<Village>()
                .Join(_uow.Set<Section>(), Village => Village.SectionId, Section => Section.Id, (Village, Section) => new { Village, Section })
                .Join(_uow.Set<City>(), Village => Village.Section.CityId, City => City.Id, (Village, City) => new { Village.Village, Village.Section, City })
                .Join(_uow.Set<State>(), Village => Village.City.StateId, State => State.Id, (Village, State) => new { Village.Village, Village.Section, Village.City, State });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListVillageViewModel()
            {
                Village = new Village() { Id = Result.Village.Id, SectionId = Result.Village.SectionId, Title = Result.Village.Title, Code = Result.Village.Code },
                Section = new Section() { Id = Result.Section.Id, CityId = Result.Section.CityId, Title = Result.Section.Title, Code = Result.Section.Code },
                City = new City() { Id = Result.City.Id, StateId = Result.City.StateId, Title = Result.City.Title, Code = Result.City.Code },
                State = new State() { Id = Result.State.Id, Title = Result.State.Title, Code = Result.State.Code }
            });
        }

    }
}
