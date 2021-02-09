using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.General
{
    [Table("Roles", Schema = "General")]
    public class Role
    {
        [Key]
        [DisplayName("شناسه نقش")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان نقش")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        [StringLength(50, MinimumLength=0, ErrorMessage="حد اکثر 50 کاراکتر")]
        [DisplayName("کد")]
        public string Code { get; set; }

        [DisplayName("سطح")]
        public int? Level { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه نقش")]
        public Int64? ParentId { get; set; }

        [DisplayName("فعال")]
        public bool? IsActive { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        [ForeignKey("ParentId")]
        public virtual Role RoleParent { get; set; }


    }
}
