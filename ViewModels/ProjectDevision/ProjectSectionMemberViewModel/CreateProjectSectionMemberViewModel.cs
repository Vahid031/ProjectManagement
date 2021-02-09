using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;
using ViewModels.Other;

namespace ViewModels.ProjectDevision.ProjectSectionMemberViewModel
{
    public class CreateProjectSectionMemberViewModel
	{
        public ProjectSectionMember ProjectSectionSupervisorMember { get; set; }
        public ProjectSectionMember ProjectSectionAdvisorMember { get; set; }

        public List<BaseDropdownViewModel> Members { get; set; }
        public List<MonitoringType> MonitoringTypes { get; set; }
	}
}