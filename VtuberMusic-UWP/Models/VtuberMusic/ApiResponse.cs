namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
    }

    public class ApiResponseList<T> : ApiResponse
    {
        public long Total { get; set; }
        public T Data { get; set; }
    }

    public class AccountProfileData
    {
        public Account account { get; set; }
        public Profile profile { get; set; }
    }

    public class LoginData : AccountProfileData
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
