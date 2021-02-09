using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionCeremonialViewModel
{
    public class CreateProjectSectionCeremonialViewModel
	{
        public ProjectSectionCeremonial ProjectSectionCeremonial { get; set; }

        public string Files { get; set; }
	}
}