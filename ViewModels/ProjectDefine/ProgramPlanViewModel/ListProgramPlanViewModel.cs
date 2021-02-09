using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProgramPlanViewModel
{
    public class ListProgramPlanViewModel
	{
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [DisplayName("شناسه برنامه")]
        public Int64? ProgramId { get; set; }

        [DisplayName("عنوان برنامه")]
        public string ProgramTitle { get; set; }

        [DisplayName("عنوان طرح")]
        public string PlanTitle { get; set; }

        [DisplayName("کد طرح")]
        public string PlanCode { get; set; }

        [DisplayName("نوع طرح")]
        public Int64? PlanTypeId { get; set; }

        [DisplayName("نوع طرح")]
        public string PlanTypeTitle { get; set; }

        [DisplayName("دستگاه اجرایی")]
        public Int64? ExecutionSetId { get; set; }

        [DisplayName("دستگاه اجرایی")]
        public string ExecutionSetTitle { get; set; }

        [DisplayName("دستگاه بهره برداری")]
        public Int64? BeneficiarySetId { get; set; }

        [DisplayName("دستگاه بهره برداری")]
        public string BeneficiarySetTitle { get; set; }

        [DisplayName("تعداد کل پروژه ها")]
        public string TotalProjects { get; set; }

        [DisplayName("تاریخ شروع اولین پروژه")]
        public Int64? FirstProjectFinantialYearId { get; set; }

        [DisplayName("تاریخ شروع اولین پروژه")]
        public string FirstProjectFinantialYearTitle { get; set; }

        [DisplayName("پیش بینی تاریخ خاتمه آخرین پروژه")]
        public Int64? ForecastLastProjectFinantialYearId { get; set; }

        [DisplayName("پیش بینی تاریخ خاتمه آخرین پروژه")]
        public string ForecastLastProjectFinantialYearTitle { get; set; }
	}
}