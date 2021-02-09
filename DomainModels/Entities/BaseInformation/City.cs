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
    [Table("Cities", Schema = "BaseInformation")]
    public class City
    {
        [Key]
        [DisplayName("شهرستان")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("استان")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? StateId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد شهرستان")]
        [Remote("CodeValidation", "City", "BaseInformation", AdditionalFields = "Id,StateId", ErrorMessage = "تکراری است")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان شهرستان")]
        [Required(ErrorMessage = "الزامی است")]
        [Remote("TitleValidation", "City", "BaseInformation", AdditionalFields = "Id,StateId", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        public virtual ICollection<Section> Sections { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
