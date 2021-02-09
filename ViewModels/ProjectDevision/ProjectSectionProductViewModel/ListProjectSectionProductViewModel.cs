using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionProductViewModel
{
    public class ListProjectSectionProductViewModel
	{
        public ProjectSectionProduct ProjectSectionProduct { get; set; }
        public ServiceProduct ServiceProduct { get; set; }
	}
}