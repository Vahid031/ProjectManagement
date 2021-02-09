using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionProducts", Schema = "ProjectDevision")]
    public class ProjectSectionProduct
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("کالا / خدمات")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? ServiceProductId { get; set; }

        [StringLength(500, MinimumLength=0, ErrorMessage="حد اکثر 500 کاراکتر")]
        [DisplayName("نوع کالا / خدمات")]
        public string ServiceProductContent { get; set; }

        [DisplayName("میزان مصرفی / حجم مصرفی")]
        public Int64? UsedAmount { get; set; }

        [DisplayName("برآورد هزینه")]
        public Int64? EstimatesPrice { get; set; }

        [ForeignKey("ServiceProductId")]
        public virtual ServiceProduct ServiceProduct { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }

        public virtual ICollection<ProjectSectionProductFile> ProjectSectionProductFiles { get; set; }
    }
}
