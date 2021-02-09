using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectReporting

{
   
    public class C1
    {
        [DisplayName("ملی")]
        public Int32? fir { get; set; }
        [DisplayName("استانی")]
        public Int32? sec { get; set; }
        [DisplayName("کل")]
        public Int32? sum { get; set; }

       
    }
}
