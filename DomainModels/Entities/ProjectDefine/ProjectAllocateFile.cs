using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectAllocateFiles", Schema = "ProjectDefine")]
    public class ProjectAllocateFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectAllocateId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectAllocateId")]
        public virtual ProjectAllocate ProjectAllocate { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

    }
}
