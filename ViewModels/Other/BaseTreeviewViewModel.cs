using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Other
{
    public class BaseTreeviewViewModel
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public Int64? ParentId { get; set; }
        public bool HaveChild { get; set; }

    }
}
