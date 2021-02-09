using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionDeliveryFixdViewModel
{
    public class ListProjectSectionDeliveryFixdViewModel
	{
        public ProjectSectionDeliveryFixd ProjectSectionDeliveryFixd { get; set; }
        public Opinion Opinion { get; set; }
        public Defect Defect { get; set; }
	}
}