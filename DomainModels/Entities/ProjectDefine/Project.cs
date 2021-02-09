using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using System.Web.Mvc;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("Projects", Schema = "ProjectDefine")]
    public class Project
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("وضعیت پروژه ")]
        public Int64? ProjectStateId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع پروژه")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? ProjectTypeId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان پروژه")]
        [Required(ErrorMessage = "الزامی است")]
        public string ProjectTitle { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد پروژه")]
        [Required(ErrorMessage = "الزامی است")]
        [Remote("FileCodeValidation", "Project", AdditionalFields = "Id", ErrorMessage = "تکراری است ")]
        public string FileCode { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نحوه اجرا")]
        public Int64? ExecutionTypeId { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("هدف پروژه")]
        public string ProjectGoal { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("بخش")]
        public Int64? SectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("روستا")]
        public Int64? VillageId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("دهستان")]
        public Int64? RuralDistrictId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("سال شروع")]
        public Int64? StartFinantialYearId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("پیش بینی سال خاتمه")]
        public Int64? ForecastEndFinantialYearId { get; set; }

        [DisplayName("مقدار")]
        public Int64? Amount { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("واحد")]
        public Int64? UnitId { get; set; }

        [DisplayName("برآورد هزینه")]
        public Int64? EstimateCost { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("وضعیت مالکیت")]
        public Int64? OwnershipTypeId { get; set; }

        [StringLength(20, MinimumLength = 0, ErrorMessage = "حد اکثر 20 کاراکتر")]
        [DisplayName("شماره")]
        public String Number { get; set; }

        [DisplayName("مساحت زمین")]
        public Int64? Area { get; set; }

        [DisplayName("مساحت سرپوشیده")]
        public Int64? CloseArea { get; set; }

        [DisplayName("مساحت سرباز")]
        public Int64? OpenArea { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره پروانه ساختمان")]
        public string BuildingPermitNumber { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره اشتراک انشعاب گاز")]
        public string GasNumber { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره اشتراک انشعاب برق")]
        public string PowerNumber { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("شماره اشتراک انشعاب آب")]
        public string WaterNumber { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("آدرس")]
        public string Address { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("موقعیت X")]
        public string LocationX { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("موقعیت Y")]
        public string LocationY { get; set; }

        [ForeignKey("StartFinantialYearId")]
        [InverseProperty("ProjectsStartFinantialYearId")]
        public virtual FinantialYear FinantialYearStartFinantialYearId { get; set; }

        [ForeignKey("ForecastEndFinantialYearId")]
        [InverseProperty("ProjectsForecastEndFinantialYearId")]
        public virtual FinantialYear FinantialYearForecastEndFinantialYearId { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        [ForeignKey("ProjectTypeId")]
        public virtual ProjectType ProjectType { get; set; }

        [ForeignKey("ExecutionTypeId")]
        public virtual ExecutionType ExecutionType { get; set; }

        [ForeignKey("OwnershipTypeId")]
        public virtual OwnershipType OwnershipType { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("ProjectStateId")]
        public virtual ProjectState ProjectState { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public virtual ICollection<ProjectFunding> ProjectFundings { get; set; }

        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }

        public virtual ICollection<ProjectAllocate> ProjectAllocates { get; set; }

        public virtual ICollection<ProjectAverage> ProjectAverages { get; set; }

        public virtual ICollection<ProjectQuantityGoal> ProjectQuantityGoals { get; set; }

        public virtual ICollection<ProjectInvestmentChapter> ProjectInvestmentChapters { get; set; }

        public virtual ICollection<ProjectSection> ProjectSections { get; set; }

        public virtual ICollection<ProjectPlan> ProjectPlans { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        [ForeignKey("VillageId")]
        public virtual Village Village { get; set; }

        [ForeignKey("RuralDistrictId")]
        public virtual RuralDistrict RuralDistrict { get; set; }


    }
}
