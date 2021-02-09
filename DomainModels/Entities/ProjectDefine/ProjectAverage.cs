using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectAverages", Schema = "ProjectDefine")]
    public class ProjectAverage
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectId { get; set; }

        [DisplayName("سال عملکرد")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? FinantialYearId { get; set; }

        [DisplayName("میانگین")]
        public float? Average { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("FinantialYearId")]
        public virtual FinantialYear FinantialYear { get; set; }

    }
}
