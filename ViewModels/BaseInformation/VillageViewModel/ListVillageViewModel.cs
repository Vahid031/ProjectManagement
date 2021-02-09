using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.BaseInformation.VillageViewModel
{
	public class ListVillageViewModel
	{
        public City City { get; set; }

        public State State { get; set; }

        public Section Section { get; set; }

		public Village Village { get; set; }
	}
}