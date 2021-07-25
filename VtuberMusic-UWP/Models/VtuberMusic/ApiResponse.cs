namespace VtuberMusic_UWP.Models.VtuberMusic
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
    }

    public class ApiResponseList<T>
    {
        public long Total { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Msg { get; set; }
    }

    public class ApiAccountProfileResponse
    {
        public AccountProfileData Data { get; set; }
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
    }

    public class AccountProfileData
    {
        public Account account { get; set; }
        public Profile profile { get; set; }
    }

    public class ApiLoginResponse : ApiAccountProfileResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
