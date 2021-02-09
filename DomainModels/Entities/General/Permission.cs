using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.General
{
    [Table("Permissions", Schema = "General")]
    public class Permission
    {
        [Key]
        [DisplayName("شناسه مجوز")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان مجوز")]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("مسیر")]
        public string Url { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه مجوز")]
        public Int64? ParentId { get; set; }

        [DisplayName("سطح")]
        public int? Level { get; set; }

        public int OrderId { get; set; }

        [StringLength(1000, MinimumLength=0, ErrorMessage="حد اکثر 1000 کاراکتر")]
        [DisplayName("آیکن")]
        public string Icon { get; set; }

        [ForeignKey("ParentId")]
        public virtual Permission PermissionParent { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }


    }
}
