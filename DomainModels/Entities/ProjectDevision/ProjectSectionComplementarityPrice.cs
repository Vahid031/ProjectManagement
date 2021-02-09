using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionComplementarityPrices", Schema = "ProjectDevision")]
    public class ProjectSectionComplementarityPrice
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("شماره")]
        public string Number { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ")]
        public string Date { get; set; }

        [DisplayName("درصد افزایش")]

        public int? IncreasePercent { get; set; }

        [DisplayName("درصد کاهش")]
        public int? DecreasePercent { get; set; }

        [DisplayName("مبلغ جدید")]
        public Int64? NewPrice { get; set; }

        public virtual ICollection<ProjectSectionComplementarityPriceFile> ProjectSectionComplementarityPriceFiles { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
