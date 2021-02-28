using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class Account
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public object Birthday { get; set; }
        public string Avatar { get; set; }
        public int Sex { get; set; }
        public string SexText { get; set; }
        public object BirthdayText { get; set; }
        public string Level { get; set; }
        public int Exp { get; set; }
        public int TotalExp { get; set; }
        public int LikeMusicTotal { get; set; }
        public int LikeVtuber { get; set; }
    }

    public class Me
    {
        public Account UserInfo { get; set; }
        public int Message { get; set; }
    }

    public class LoginResponse
    {
        public string Data { get; set; }
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
    }
}
