using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using DomainModels.Entities.General;

namespace DomainModels.Entities.BaseInformation
{
    [Table("FileTypes", Schema = "BaseInformation")]
    public class FileType
    {
        [Key]
        [DisplayName("شناسه")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
