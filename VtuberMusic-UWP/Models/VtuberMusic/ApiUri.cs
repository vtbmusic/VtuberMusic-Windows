using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VtuberMusic_UWP.Models.VtuberMusic
{
    class ApiUri
    {
        /// <summary>
        /// Api 地址
        /// </summary>
        public static string BaseUrl = "https://api.aqua.chat";

        /// <summary>
        /// 获取 CDN 列表
        /// </summary>
        public static string GetCDNList = BaseUrl + "/v1/GetCDNList";

        #region 音乐
        #region 音乐数据
        /// <summary>
        /// 获取音乐列表
        /// </summary>
        public static string GetMusicList = BaseUrl + "/v1/GetMusicList";

        /// <summary>
        /// 获取热门音乐列表
        /// </summary>
        public static string GetHotMusicList = BaseUrl + "/v1/GetHotMusicList";

        /// <summary>
        /// 获取收藏歌曲列表
        /// </summary>
        public static string GetLikeMusicList = BaseUrl + "/v1/GetLikeMusicList";

        /// <summary>
        /// 获取单条 / 多条音乐数据
        /// </summary>
        public static string GetMusicData = BaseUrl + "/v1/GetMusicData";

        /// <summary>
        /// 获取主推音乐列表
        /// </summary>
        public static string GetLikeVtuberMusicList = BaseUrl + "/v1/GetSubMusicList";
        #endregion

        #region 喜欢 / 取消喜欢音乐
        /// <summary>
        /// 批量喜欢 / 取消喜欢音乐
        /// </summary>
        public static string LikeMusics = BaseUrl + "/v1/LikeMusicDatas";

        /// <summary>
        /// 喜欢 / 取消喜欢音乐
        /// </summary>
        public static string LikeMusic = BaseUrl + "/v1/LikeMusicData";
        #endregion

        #region 音乐评论
        /// <summary>
        /// 获取音乐评论列表
        /// </summary>
        public static string GetMusicCommentList = BaseUrl + "/v1/GetMusicCommentList";

        /// <summary>
        /// 添加音乐评论
        /// </summary>
        public static string AddMusicComment = BaseUrl + "/v1/AddMusicComment";
        #endregion
        #endregion

        #region Vtuber
        #region Vtuber 数据
        /// <summary>
        /// 获取 Vtuber 列表
        /// </summary>
        public static string GetVtbsList = BaseUrl + "/v1/GetVtbsList";

        /// <summary>
        /// 获取 Vtuber 数据
        /// </summary>
        public static string GetVtbsData = BaseUrl + "/v1/GetVtbsData";

        /// <summary>
        /// 获取主推列表
        /// </summary>
        public static string GetLikeVtbsList = BaseUrl + "/v1/GetLikeVtbsList";
        #endregion

        /// <summary>
        /// 喜欢 / 取消喜欢 Vtuber
        /// </summary>
        public static string LikeVtbsData = BaseUrl + "/v1/LikeVtbsData";
        #endregion

        #region 歌单
        #region 歌单数据
        /// <summary>
        /// 获取歌单列表
        /// </summary>
        public static string GetAlbumsList = BaseUrl + "/v1/GetAlbumsList";

        /// <summary>
        /// 获取喜欢歌单列表
        /// </summary>
        public static string GetLikeAlbumsList = BaseUrl + "/v1/GetLikeAlbumsList";

        /// <summary>
        /// 获取歌单详细数据
        /// </summary>
        public static string GetAlbumsData = BaseUrl + "/v1/GetAlbumsData";
        #endregion

        /// <summary>
        /// 喜欢 / 取消喜欢歌单
        /// </summary>
        public static string LikeAlbumsData = BaseUrl + "/v1/LikeAlbumsData";
        #endregion

        #region 群组
        /// <summary>
        /// 获取群组列表
        /// </summary>
        public static string GetGroupsList = BaseUrl + "/v1/GetGroupsList";

        /// <summary>
        /// 获取群组详细信息
        /// </summary>
        public static string GetGroupsData = BaseUrl + "/v1/GetGroupsData";
        #endregion

        #region 用户
        /// <summary>
        /// 获取已登录用户信息
        /// </summary>
        public static string GetAccountInfo = BaseUrl + "/v1/GetNavInfo";

        /// <summary>
        /// 注册
        /// </summary>
        public static string Register = BaseUrl + "/v1/Register";

        /// <summary>
        /// 重置密码
        /// </summary>
        public static string RetrievePassword = BaseUrl + "/v1/RetrievePassword";

        /// <summary>
        /// 获取验证码
        /// </summary>
        public static string GetCaptcha = BaseUrl + "/v1/GetCaptcha";

        /// <summary>
        /// 登录
        /// </summary>
        public static string Login = BaseUrl + "/v1/SubmitLogin";
        #endregion

        public static string GetBannerData = BaseUrl + "/v1/GetDataList";
    }
}
