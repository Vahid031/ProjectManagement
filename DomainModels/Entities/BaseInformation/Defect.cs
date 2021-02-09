using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Defects", Schema = "BaseInformation")]
    public class Defect
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        public virtual ICollection<ProjectSectionDeliveryTemprory> ProjectSectionDeliveryTemprories { get; set; }

        public virtual ICollection<ProjectSectionDeliveryFixd> ProjectSectionDeliveryFixdes { get; set; }


    }
}
