using System;
using System.Collections.Generic;
using System.Text;

namespace VtuberMusic.Core.Models {
    public class Account {
        public string id { get; set; }
        public string userName { get; set; }
        public int createTime { get; set; }
        public int ban { get; set; }
        public int lastLoginTime { get; set; }
        public string lastLoginIP { get; set; }
        public int type { get; set; }
        public int init { get; set; }
        public AccountBind bind { get; set; }
        public int message { get; set; }
    }

    public class AccountBind {
        public bool qq { get; set; }
    }
}
