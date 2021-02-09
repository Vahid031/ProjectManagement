using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;

namespace DomainModels.Entities.BaseInformation
{
    [Table("MemberGroups", Schema = "BaseInformation")]
    public class MemberGroup
    {
        [Key]
        [DisplayName("شناسه  گروه کاربری")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان  گروه کاربری")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        public virtual ICollection<Member> Members { get; set; }


    }
}
