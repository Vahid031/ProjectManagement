using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDevision.ProjectSectionLiberalizationViewModel
{
    public class ListProjectSectionLiberalizationViewModel
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

        [DisplayName("شماره قرارداد")]
        public string ContractNumber { get; set; }

        [DisplayName("پیمانکار")]
        public string CompanyName { get; set; }

        [DisplayName("تاریخ شروع")]
        public string StartDate { get; set; }

        [DisplayName("تاریخ پایان")]
        public string EndDate { get; set; }
	}
}