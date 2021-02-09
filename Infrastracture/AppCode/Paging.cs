using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastracture.AppCode
{
    public class Paging
    {
        public int _pageNumber { get; set; }
        public int _pageSize { get; set; }
        public string _orderColumn { get; set; }
        public Int64 _rowCount { get; set; }

        private string Filter = "";
        public string _filter 
        {
            get {
                return Filter;
            }
            set { Filter = value; } 
        }

        public List<object> _values = new List<object>();
    }
}