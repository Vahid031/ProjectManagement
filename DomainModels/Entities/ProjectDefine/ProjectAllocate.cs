using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectAllocates", Schema = "ProjectDefine")]
    public class ProjectAllocate
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectId { get; set; }

        [DisplayName("فاز پروژه")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("طرح پروژه")]
        public Int64? ProjectPlanId { get; set; }

        [DisplayName("مبلغ اعتبار")]
        public Int64? CreditAmount { get; set; }

        [DisplayName("مبلغ تخصیص")]
        public Int64? AllocateAmount { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ تخصیص")]
        public string AllocateDate { get; set; }

        [DisplayName("درصد اعتبار")]
        public int? CreditPercent { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


        [ForeignKey("ProjectPlanId")]
        public virtual ProjectPlan ProjectPlan { get; set; }

        public virtual ICollection<ProjectAllocateFile> ProjectAllocateFiles { get; set; }

    }
}
