using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel
{
    public class ListProjectSectionSupervisorVisitViewModel
	{
        public ProjectSectionSupervisorVisit ProjectSectionSupervisorVisit { get; set; }
        public ProjectSectionOperationTitle ProjectSectionOperationTitle { get; set; }
        public ProjectSectionMember ProjectSectionMember { get; set; }
        public Member Member { get; set; }
	}
}