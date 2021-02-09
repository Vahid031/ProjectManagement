using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;
using ViewModels.Other;

namespace ViewModels.ProjectDevision.ProjectSectionAdvisorVisitViewModel
{
    public class CreateProjectSectionAdvisorVisitViewModel
	{
        public ProjectSectionAdvisorVisit ProjectSectionAdvisorVisit { get; set; }

        public string Files { get; set; }

        public List<BaseDropdownViewModel> Members { get; set; }
	}
}