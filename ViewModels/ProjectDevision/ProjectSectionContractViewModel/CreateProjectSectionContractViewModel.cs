using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionContractViewModel
{
    public class CreateProjectSectionContractViewModel
	{
        public ProjectSectionContract ProjectSectionContract { get; set; }
        public ProjectSectionContractDetail ProjectSectionContractDetail { get; set; }

        public string Files { get; set; }
        public string ContractDetails { get; set; }

        public List<ContractType> ContractTypes { get; set; }
        public List<Unit> Units { get; set; }
	}
}