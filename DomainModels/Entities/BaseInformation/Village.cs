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
    [Table("Villages", Schema = "BaseInformation")]
    public class Village
    {
        [Key]
        [DisplayName("شناسه")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("بخش")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? SectionId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد روستا")]
        [Remote("CodeValidation", "Village", "BaseInformation", AdditionalFields = "Id,SectionId", ErrorMessage = "تکراری است")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان روستا")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "Village", "BaseInformation", AdditionalFields = "Id,SectionId", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<RuralDistrict> RuralDistricts { get; set; }


    }
}
