using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionInquiryViewModel
{
    public class CreateProjectSectionInquiryViewModel
	{
        public ProjectSectionInquiry ProjectSectionInquiry { get; set; }

        public string Files { get; set; }
	}
}