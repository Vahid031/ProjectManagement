using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.General
{
    [Table("RolePermissions", Schema = "General")]
    public class RolePermission
    {
        [Key]
        [DisplayName("شناسه مجوز های نقش")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه نقش")]
        public Int64? RoleId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه مجوز")]
        public Int64? PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }


    }
}
