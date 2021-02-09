using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.BaseInformation.CityViewModel
{
    public class CreateCityViewModel
	{
        public City City { get; set; }
        public List<State> States { get; set; }
	}
}