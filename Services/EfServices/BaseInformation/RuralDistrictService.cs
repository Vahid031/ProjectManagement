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
using ViewModels.BaseInformation.RuralDistrictViewModel;

namespace Services.EfServices.BaseInformation
{
    public class RuralDistrictService : GenericService<RuralDistrict>, IRuralDistrictService
    {
        public RuralDistrictService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListRuralDistrictViewModel> GetAll(ListRuralDistrictViewModel lrdvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListRuralDistrictViewModel>(lrdvm, ref pg);

            var a =
            _uow.Set<RuralDistrict>()
                .Join(_uow.Set<Village>(), RuralDistrict => RuralDistrict.VillageId, Village => Village.Id, (RuralDistrict, Village) => new { RuralDistrict, Village })
                .Join(_uow.Set<Section>(), RuralDistrict => RuralDistrict.Village.SectionId, Section => Section.Id, (RuralDistrict, Section) => new { RuralDistrict.RuralDistrict, RuralDistrict.Village, Section })
                .Join(_uow.Set<City>(), RuralDistrict => RuralDistrict.Section.CityId, City => City.Id, (RuralDistrict, City) => new { RuralDistrict.RuralDistrict, RuralDistrict.Village, RuralDistrict.Section, City })
                .Join(_uow.Set<State>(), RuralDistrict => RuralDistrict.City.StateId, State => State.Id, (RuralDistrict, State) => new { RuralDistrict.RuralDistrict, RuralDistrict.Village, RuralDistrict.Section, RuralDistrict.City, State });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListRuralDistrictViewModel()
            {
                RuralDistrict = new RuralDistrict() { Id = Result.RuralDistrict.Id, VillageId = Result.RuralDistrict.VillageId, Title = Result.RuralDistrict.Title, Code = Result.RuralDistrict.Code },
                Village = new Village() { Id = Result.Village.Id, SectionId = Result.Village.SectionId, Title = Result.Village.Title, Code = Result.Village.Code },
                Section = new Section() { Id = Result.Section.Id, CityId = Result.Section.CityId, Title = Result.Section.Title, Code = Result.Section.Code },
                City = new City() { Id = Result.City.Id, StateId = Result.City.StateId, Title = Result.City.Title, Code = Result.City.Code },
                State = new State() { Id = Result.State.Id, Title = Result.State.Title, Code = Result.State.Code }
            });
        }
    }
}
