using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectPlans", Schema = "ProjectDefine")]
    public class ProjectPlan
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("پروژه")]
        public Int64? ProjectId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("برنامه")]
        public Int64? ProgramId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("طرح برنامه")]
        public Int64? ProgramPlanId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("محل تامین اعتبار")]
        public Int64? CreditProvidePlaceId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("از سال")]
        public Int64? FromFinantialYearId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("تا سال")]
        public Int64? ToFinantialYearId { get; set; }


        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        [ForeignKey("ProgramPlanId")]
        public virtual ProgramPlan ProgramPlan { get; set; }

        [ForeignKey("CreditProvidePlaceId")]
        public virtual CreditProvidePlace CreditProvidePlace { get; set; }

        [ForeignKey("FromFinantialYearId")]
        [InverseProperty("ProjectPlansFromFinantialYearId")]
        public virtual FinantialYear FinantialYearFromFinantialYearId { get; set; }

        [ForeignKey("ToFinantialYearId")]
        [InverseProperty("ProjectPlansToFinantialYearId")]
        public virtual FinantialYear FinantialYearToFinantialYearId { get; set; }

        public virtual ICollection<ProjectFunding> ProjectFundings { get; set; }
        public virtual ICollection<ProjectAllocate> ProjectAllocates { get; set; }
    }
}
