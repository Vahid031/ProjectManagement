using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.BaseInformation.RuralDistrictViewModel
{
	public class ListRuralDistrictViewModel
    {
        public City City { get; set; }

        public State State { get; set; }

        public Section Section { get; set; }

        public Village Village { get; set; }

		public RuralDistrict RuralDistrict { get; set; }
	}
}