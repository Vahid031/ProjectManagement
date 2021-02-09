using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionMadeh48Files", Schema = "ProjectDevision")]
    public class ProjectSectionMadeh48File
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionMadeh48Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionMadeh48Id")]
        public virtual ProjectSectionMadeh48 ProjectSectionMadeh48 { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }
    }
}
