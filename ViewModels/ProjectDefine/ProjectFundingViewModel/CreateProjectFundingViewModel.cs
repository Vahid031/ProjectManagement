using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using ViewModels.Other;

namespace ViewModels.ProjectDefine.ProjectFundingViewModel
{
    public class CreateProjectFundingViewModel
	{
        public ProjectFunding ProjectFunding { get; set; }

        [DisplayName("منبع اعتبار")]
        public Int64? ResourceId { get; set; }
        public string Files { get; set; }

        public List<FinantialYear> FinantialYears { get; set; }
        public List<Resource> Resources { get; set; }
        public List<ResourceType> ResourceTypes { get; set; }
        public List<BaseDropdownViewModel> ProjectPlans { get; set; }
	}
}