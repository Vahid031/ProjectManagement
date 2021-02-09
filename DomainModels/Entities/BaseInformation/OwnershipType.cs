using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;

namespace DomainModels.Entities.BaseInformation
{
    [Table("OwnershipTypes", Schema = "BaseInformation")]
    public class OwnershipType
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        public string Title { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
