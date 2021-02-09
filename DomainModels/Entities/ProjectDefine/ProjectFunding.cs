using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectFundings", Schema = "ProjectDefine")]
    public class ProjectFunding
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("طرح پروژه")]
        public Int64? ProjectPlanId { get; set; }

        [DisplayName("سال عملکرد")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64? FinantialYearId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نوع منبع اعتبار")]
        public Int64? ResourceTypeId { get; set; }

        [DisplayName("مبلغ")]
        public Int64? Price { get; set; }

        [ForeignKey("ResourceTypeId")]
        public virtual ResourceType ResourceType { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("FinantialYearId")]
        public virtual FinantialYear FinantialYear { get; set; }

        [ForeignKey("ProjectPlanId")]
        public virtual ProjectPlan ProjectPlan { get; set; }

        public virtual ICollection<ProjectFundingFile> ProjectFundingFiles { get; set; }
    }
}
