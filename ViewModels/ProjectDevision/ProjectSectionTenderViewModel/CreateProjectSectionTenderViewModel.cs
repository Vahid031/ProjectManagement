using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionTenderViewModel
{
    public class CreateProjectSectionTenderViewModel
	{
        public ProjectSectionTender ProjectSectionTender { get; set; }

        public string Files { get; set; }
	}
}