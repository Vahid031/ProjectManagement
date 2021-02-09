using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionProductViewModel
{
    public class CreateProjectSectionProductViewModel
	{
        public ProjectSectionProduct ProjectSectionProduct { get; set; }
        public string Files { get; set; }
        public List<ServiceProduct> ServiceProducts { get; set; }
	}
}