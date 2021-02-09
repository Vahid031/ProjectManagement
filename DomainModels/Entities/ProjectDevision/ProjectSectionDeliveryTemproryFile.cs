using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionDeliveryTemproryFiles", Schema = "ProjectDevision")]
    public class ProjectSectionDeliveryTemproryFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionDeliveryTemproryId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionDeliveryTemproryId")]
        public virtual ProjectSectionDeliveryTemprory ProjectSectionDeliveryTemprory { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

    }
}
