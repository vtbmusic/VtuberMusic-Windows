using PropertyChanged;
using System.ComponentModel;

namespace VtuberMusic_UWP.Models.VtuberMusic {
    /// <summary>
    /// Api 响应
    /// </summary>
    /// <typeparam name="T">Data Type</typeparam>
    public class ApiResponse<T> : ApiResponse {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// Api 响应
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class ApiResponse : INotifyPropertyChanged {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string Msg { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Api 数据列表响应
    /// </summary>
    /// <typeparam name="T">Data Type</typeparam>
    public class ApiResponseList<T> : ApiResponse {
        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 账户资料和信息响应 Object
    /// </summary>
    public class AccountProfileData {
        /// <summary>
        /// 账户信息
        /// </summary>
        public Account account { get; set; }
        /// <summary>
        /// 账户资料
        /// </summary>
        public Profile profile { get; set; }
    }

    /// <summary>
    /// 账户登录响应
    /// </summary>
    public class LoginData : AccountProfileData {
        /// <summary>
        /// Access Token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// Refresh Token
        /// </summary>
        public string refresh_token { get; set; }
    }
}
