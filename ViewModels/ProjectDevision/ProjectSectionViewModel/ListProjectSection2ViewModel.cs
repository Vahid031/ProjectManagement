using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionViewModel
{
    public class ListProjectSection2ViewModel
	{
        public long ProjectId { get; set; }
        public long Id { get; set; }
        public string ProjectSectionTitle { get; set; }
        public string ForecastStartDate { get; set; }
        public string ForecastEndDate { get; set; }
        public long? TotalProductServicePrice { get; set; }

        public string AssignmentTypeTitle { get; set; }

	}
   
}