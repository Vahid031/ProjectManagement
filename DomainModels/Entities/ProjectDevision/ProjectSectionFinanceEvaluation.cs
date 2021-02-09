using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionFinanceEvaluations", Schema = "ProjectDevision")]
    public class ProjectSectionFinanceEvaluation
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("پیمانکار")]
        public Int64? ContractorId { get; set; }

        [DisplayName("نمره ارزیابی")]
        public Int64? ScoreEvaluation { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("شرح")]
        public string Description { get; set; }

        [ForeignKey("ContractorId")]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
