using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProjectAgreementViewModel
{
    public class ListProjectAgreementViewModel
	{
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }
        
        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }
        
        [DisplayName("نوع پروژه")]
        public Int64? ProjectTypeId { get; set; }

        [DisplayName("نوع پروژه")]
        public string ProjectTypeTitle { get; set; }

        [DisplayName("تاریخ شروع")]
        public Int64? StartFinantialYearId { get; set; }

        [DisplayName("تاریخ شروع")]
        public string StartFinantialYearTitle { get; set; }

        [DisplayName("پیش بینی تاریخ پایان")]
        public Int64? ForecastEndFinantialYearId { get; set; }

        [DisplayName("پیش بینی تاریخ پایان")]
        public string ForecastEndFinantialYearTitle { get; set; }

        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [DisplayName("استان")]
        public string StateTitle { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("شهرستان")]
        public string CityTitle { get; set; }

        [DisplayName("بخش")]
        public Int64? SectionId { get; set; }

        [DisplayName("بخش")]
        public string SectionTitle { get; set; }

        [DisplayName("روستا")]
        public Int64? VillageId { get; set; }

        [DisplayName("روستا")]
        public string VillageTitle { get; set; }

        [DisplayName("دهستان")]
        public Int64? RuralDistrictId { get; set; }

        [DisplayName("دهستان")]
        public string RuralDistrictTitle { get; set; }

        [DisplayName("مقدار")]
        public string Amount { get; set; }

        [DisplayName("واحد")]
        public Int64? UnitId { get; set; }

        [DisplayName("واحد")]
        public string UnitTitle { get; set; }

        [DisplayName("وضعیت پروژه")]
        public Int64? ProjectStateId { get; set; }

        [DisplayName("وضعیت پروژه")]
        public string ProjectStateTitle { get; set; }

        [DisplayName("رنگ")]
        public string Color { get; set; }
        //public int StatementStatus { get; set; }
	}
}