using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDevision.ProjectSectionAssignmentViewModel
{
    public class ListProjectSectionAssignmentViewModel
	{
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [DisplayName("پروژه")]
        public Int64? ProjectId { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }
        
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }
        
        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [DisplayName("استان")]
        public string StateTitle { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("شهرستان")]
        public string CityTitle { get; set; }

        [DisplayName("عنوان فاز")]
        public string ProjectSectionTitle { get; set; }

        [DisplayName("نحوه واگذاری")]
        public Int64? AssignmentTypeId { get; set; }

        [DisplayName("نحوه واگذاری")]
        public string AssignmentTypeTitle { get; set; }

        [DisplayName("وضعیت پروژه")]
        public Int64? ProjectSectionAssignmentStateId { get; set; }

        [DisplayName("وضعیت پروژه")]
        public string ProjectSectionAssignmentStateTitle { get; set; }

        [DisplayName("رنگ")]
        public string Color { get; set; }
	}
}