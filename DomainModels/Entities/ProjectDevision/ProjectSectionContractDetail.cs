using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionContractDetails", Schema = "ProjectDevision")]
    public class ProjectSectionContractDetail
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionContractId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ریز پروژه")]
        public string ProjectDetails { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("واحد کار")]
        public Int64? UnitId { get; set; }

        [DisplayName("مقدار کار")]
        public Int64? TotalAmonts { get; set; }

        [DisplayName("هزینه کل")]
        public Int64? TotalPrice { get; set; }

        [DisplayName("هزینه واحد")]
        public Int64? UnitPrice { get; set; }

        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }

        [ForeignKey("ProjectSectionContractId")]
        public virtual ProjectSectionContract ProjectSectionContract { get; set; }


    }
}
