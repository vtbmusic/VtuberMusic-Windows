namespace VtuberMusic.Core.Models {
    public class ApiResponse {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string Msg { get; set; }
    }

    public class ApiResponse<T> : ApiResponse {
        public T Data { get; set; }
    }

    public class PlaylistMusicsResponse {
        public Playlist playlist { get; set; }
        public Music[] songs { get; set; }
    }

    public class ProfileResponse {
        public Profile profile { get; set; }
    }

    public class AccountProfileResponse : ProfileResponse {
        public Account account { get; set; }
    }

    public class LoginResponse : AccountProfileResponse {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    public class CommentResponse {
        public Comment[] hotComments { get; set; }
        public Comment[] comments { get; set; }
        public long total { get; set; }
    }
}
