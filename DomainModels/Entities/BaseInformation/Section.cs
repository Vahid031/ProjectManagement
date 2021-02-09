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
    [Table("Sections", Schema = "BaseInformation")]
    public class Section
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شهر")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? CityId { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("کد بخش")]
        [Remote("CodeValidation", "Section", "BaseInformation", AdditionalFields = "Id,CityId", ErrorMessage = "تکراری است")]
        public string Code { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان بخش")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "Section", "BaseInformation", AdditionalFields = "Id,CityId", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Village> Villages { get; set; }


    }
}
