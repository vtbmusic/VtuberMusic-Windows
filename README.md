# VtuberMusic Windows
基于 WinUI 3 开发的 VtuberMusic Windows 客户端

## 项目结构
> 注: 该项目使用 Refit 作为请求类库
```
──src
    ├─VtuberMusic.App // 应用 UI 交互层
    │  ├─Assets // 应用资源
    │  │  └─Icon
    │  │      ├─AppIcon // 用于应用程序包的图标
    │  │      └─Original // 图标原图
    │  ├─Controls // 控件
    │  │  ├─DataItem // 各类数据的 Item 控件
    │  │  ├─FirendPanel // 好友面板控件，如: 关注/粉丝 等
    │  │  ├─Lyric // 歌词控件，如: LyricItem 和 LyricView
    │  │  └─SearchPanel // 搜索面板控件，如: 歌曲/歌手/用户等
    │  ├─Converters // UI 绑定的 Converter 转换器
    │  ├─Dialogs // ContentDialog 对话框中的控件
    │  ├─Helper
    │  ├─Messages // 负责 MVVM 的 CommunityToolkit.MVVM 包里的消息实体
    │  ├─Models // 实体类，如 NavigationTag
    │  ├─PageArgs // 页面传参的参数实体类
    │  ├─Pages // 页面
    │  ├─Services // 服务，如 NavigatoinSerivce
    │  └─ViewModels // ViewModel，子目录命名同上
    │      ├─App // 所有 ViewModel 的基类
    │      ├─FriendsPanel
    │      ├─Lyric
    │      └─SearchPanel
    ├─VtuberMusic.AppCore // 应用通用服务层
    │  ├─Enums
    │  ├─Helper // Helper
    │  ├─Messages // 负责 MVVM 的 CommunityToolkit.MVVM 包里的消息实体，如: PlaybackMusicChangedMessage、PlaybackPlaylistModeChangedMessage 等
    │  └─Services // 服务，如: MediaPlaybackService
    └─VtuberMusic.Core // 核心服务层
        ├─Enums
        ├─Helper
        ├─Messages // MVVM 的 CommunityToolkit.MVVM 包里的消息实体，如: AuthorizationStateChangedMessage 等
        ├─Models // 实体类，如: ApiResponse、Profile、Music 等
        │  └─Lyric // 歌词的实体类，如: Vrc、LyricWords 等
        └─Services // 服务，如: AuthorizationService，IVtuberMusicService 等
```