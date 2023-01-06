using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class FilterViewModel
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDesc { get; set; }
        public string? Status { get; set; }
        public string? EntityStatus { get; set; }
        public FilterViewModel()
        {
            this.PageNo = 1;
            this.PageSize = 10;
        }
        public FilterViewModel(int pageNumber, int pageSize)
        {
            this.PageNo = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
