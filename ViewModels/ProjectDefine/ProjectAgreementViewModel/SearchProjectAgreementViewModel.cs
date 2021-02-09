using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProjectAgreementViewModel
{
    public class SearchProjectAgreementViewModel
	{
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("نوع پروژه")]
        public Int64? ProjectTypeId { get; set; }

        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }
        
        [DisplayName("وضعیت پروژه")]
        public Int64? ProjectStateId { get; set; }

        public List<ProjectType> ProjectTypes { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }
        public List<ProjectState> ProjectStates { get; set; }
	}
}