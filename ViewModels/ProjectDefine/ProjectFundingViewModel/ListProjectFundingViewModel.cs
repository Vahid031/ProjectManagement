using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProjectFundingViewModel
{
    public class ListProjectFundingViewModel
	{
        public ProjectFunding ProjectFunding { get; set; }
        public Resource Resource { get; set; }
        public ResourceType ResourceType { get; set; }
        public FinantialYear FinantialYear { get; set; }
	}
}