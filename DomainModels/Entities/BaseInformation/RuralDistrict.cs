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
    [Table("RuralDistricts", Schema = "BaseInformation")]
    public class RuralDistrict
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("روستا")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? VillageId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد دهستان")]
        [Remote("CodeValidation", "RuralDistrict", "BaseInformation", AdditionalFields = "Id,VillageId", ErrorMessage = "تکراری است")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان دهستان")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "RuralDistrict", "BaseInformation", AdditionalFields = "Id,VillageId", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [ForeignKey("VillageId")]
        public virtual Village Village { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
