using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Other
{
    public class UploadFilesResultViewModel
    {
        public string url { get; set; }
        public string thumbnail_url { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string delete_url { get; set; }
        public string delete_type { get; set; }
        public Int64? imageId { get; set; }
    }

}
