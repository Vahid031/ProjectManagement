using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.BaseInformation.SectionViewModel
{
    public class ListSectionViewModel
	{
        public City City { get; set; }

        public State State { get; set; }

        public Section Section { get; set; }
	}
}