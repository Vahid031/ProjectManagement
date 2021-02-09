using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("FinantialYears", Schema = "BaseInformation")]
    public class FinantialYear
    {
        [Key]
        [DisplayName("سال مالی ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان سال مالی")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "FinantialYear", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [DisplayName("فعال")]
        [Required(ErrorMessage= "الزامی است")]
        public bool? IsActive { get; set; }

        public virtual ICollection<Project> ProjectsStartFinantialYearId { get; set; }

        public virtual ICollection<Project> ProjectsForecastEndFinantialYearId { get; set; }

        public virtual ICollection<ProjectFunding> ProjectFundings { get; set; }

        public virtual ICollection<ProjectInvestmentChapter> ProjectInvestmentChapters { get; set; }

        public virtual ICollection<ProjectAverage> ProjectAverages { get; set; }

        public virtual ICollection<ProgramPlan> ProgramPlansFirstProjectFinantialYearId { get; set; }

        public virtual ICollection<ProgramPlan> ProgramPlansForecastLastProjectFinantialYearId { get; set; }

        public virtual ICollection<ProjectPlan> ProjectPlansFromFinantialYearId { get; set; }

        public virtual ICollection<ProjectPlan> ProjectPlansToFinantialYearId { get; set; }

    }
}
