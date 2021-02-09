using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("DocumentTypes", Schema = "BaseInformation")]
    public class DocumentType
    {
        [Key]
        [DisplayName("نوع مدرک")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان نوع مدرک")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "DocumentType", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }


    }
}
