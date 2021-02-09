using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;

namespace DomainModels.Entities.BaseInformation
{
    [Table("ProjectStates", Schema = "BaseInformation")]
    public class ProjectState
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        public string Title { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("رنگ")]
        public string Color { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
