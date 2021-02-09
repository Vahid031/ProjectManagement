using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDevision.ProjectSectionOperationViewModel
{
    public class SearchProjectSectionOperationViewModel
	{
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("عنوان فاز")]
        public string ProjectSectionTitle { get; set; }

        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("وضعیت صورت وضعیت")]
        public Int64? ProjectSectionOperationStateId { get; set; }

        public List<ProjectSectionOperationState> ProjectSectionOperationStates { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }
	}
}