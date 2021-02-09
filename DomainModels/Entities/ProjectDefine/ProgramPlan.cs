using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.ReqularExpression;
using System.Web.Mvc;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProgramPlans", Schema = "ProjectDefine")]
    public class ProgramPlan
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("عنوان برنامه")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? ProgramId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان طرح")]
        [Required(ErrorMessage = "الزامی است")]
        public string PlanTitle { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [Remote("PlanCodeValidation", "ProgramPlan", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        [DisplayName("کد طرح")]
        [Required(ErrorMessage = "الزامی است")]
        public string PlanCode { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع طرح")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? PlanTypeId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("دستگاه اجرایی")]
        public Int64? ExecutionSetId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("دستگاه بهره برداری")]
        public Int64? BeneficiarySetId { get; set; }

        [DisplayName("شماره اصلاحیه")]
        public Int64? RevisionNumber { get; set; }

        [RegularExpression(CustomRegex.PERSIAN_DATE, ErrorMessage = "تاریخ را بدرستی وارد کنید")]
        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ مبادله موافقتنامه")]
        public string AgreementDate { get; set; }

        [DisplayName("تعداد پروژه های مبادله شده سال جاری")]
        public int? ExchangeProjectCounts { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره اصلاحیه سال جاری")]
        public string CorrigendumNumber { get; set; }

        [DisplayName("تعداد کل پروژه ها")]
        public int? TotalProjects { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("تاریخ شروع اولین پروژه")]
        public Int64? FirstProjectFinantialYearId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("پیش بینی تاریخ خاتمه آخرین پروژه")]
        public Int64? ForecastLastProjectFinantialYearId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نوع محل اجرا")]
        public string ExecutionPlaceType { get; set; }

        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        [ForeignKey("ExecutionSetId")]
        [InverseProperty("ProgramPlansExecutionSetId")]
        public virtual Set SetExecutionSetId { get; set; }

        [ForeignKey("BeneficiarySetId")]
        [InverseProperty("ProgramPlansBeneficiarySetId")]
        public virtual Set SetBeneficiarySetId { get; set; }

        [ForeignKey("PlanTypeId")]
        public virtual PlanType PlanType { get; set; }

        public virtual ICollection<ProjectPlan> ProjectPlans { get; set; }

        [ForeignKey("FirstProjectFinantialYearId")]
        [InverseProperty("ProgramPlansFirstProjectFinantialYearId")]
        public virtual FinantialYear FinantialYearFirstProjectFinantialYearId { get; set; }

        [ForeignKey("ForecastLastProjectFinantialYearId")]
        [InverseProperty("ProgramPlansForecastLastProjectFinantialYearId")]
        public virtual FinantialYear FinantialYearForecastLastProjectFinantialYearId { get; set; }

    }
}
