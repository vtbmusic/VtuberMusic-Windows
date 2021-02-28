using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class LoginRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class ListRequest
    {
        public Search search { get; set; }
        public int pageIndex { get; set; }
        public int pageRows { get; set; }
        public string sortField { get; set; }
        public string sortType { get; set; }
    }

    public class ListRequestUpper
    {
        public Search Search { get; set; }
        public int PageIndex { get; set; }
        public int PageRows { get; set; }
        public string SortField { get; set; }
        public string SortType { get; set; }
    }

    public class Search
    {
        public string condition { get; set; }
        public string keyword { get; set; }
    }

    public class Ids
    {
        public string[] ids { get; set; }
    }
}
