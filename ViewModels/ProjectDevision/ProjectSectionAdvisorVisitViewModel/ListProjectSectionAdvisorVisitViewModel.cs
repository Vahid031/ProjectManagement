﻿using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace ViewModels.ProjectDevision.ProjectSectionAdvisorVisitViewModel
{
    public class ListProjectSectionAdvisorVisitViewModel
	{
        public ProjectSectionAdvisorVisit ProjectSectionAdvisorVisit { get; set; }
        public ProjectSectionMember ProjectSectionMember { get; set; }
        public Member Member { get; set; }
	}
}