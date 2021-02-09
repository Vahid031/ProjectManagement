using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using ViewModels.Other;

namespace ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel
{
    public class CreateProjectSectionSupervisorVisitViewModel
	{
        public ProjectSectionSupervisorVisit ProjectSectionSupervisorVisit { get; set; }

        public string Files { get; set; }

        public List<BaseDropdownViewModel> Members { get; set; }
        public List<ProjectSectionOperationTitle> ProjectSectionOperationTitles { get; set; }
	}
}