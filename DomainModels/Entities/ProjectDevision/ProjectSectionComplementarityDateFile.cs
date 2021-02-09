using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionComplementarityDateFiles", Schema = "ProjectDevision")]
    public class ProjectSectionComplementarityDateFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionComplementarityDateId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionComplementarityDateId")]
        public virtual ProjectSectionComplementarityDate ProjectSectionComplementarityDate { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }
    }
}
