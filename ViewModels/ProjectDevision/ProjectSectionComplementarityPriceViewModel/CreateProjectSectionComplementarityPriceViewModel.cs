using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionComplementarityPriceViewModel
{
    public class CreateProjectSectionComplementarityPriceViewModel
	{
        public ProjectSectionComplementarityPrice ProjectSectionComplementarityPrice { get; set; }
        public long ComplementarityPrice { get; set; }
        public long ContractPrice { get; set; }
        public long TotalIPercent { get; set; }
        public long TotalDPercent { get; set; }
        public string Files { get; set; }
	}
}