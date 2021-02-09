using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionRecoupmentFiles", Schema = "ProjectDevision")]
    public class ProjectSectionRecoupmentFile
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 ProjectSectionRecoupmentId { get; set; }


        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }


        [ForeignKey("FileId")]
        public virtual File File { get; set; }


        [ForeignKey("ProjectSectionRecoupmentId")]
        public virtual ProjectSectionRecoupment ProjectSectionRecoupment { get; set; }

    }
}
