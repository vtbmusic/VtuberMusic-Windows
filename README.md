# VtuberMusic-UWP
该为重写后的 UWP 客户端仓库，原先仓库地址: [VTuberMusic-UWP-Old](https://github.com/vtbmusic/VTuberMusic-UWP-Old)

## 获取应用
> 按更新速度排序

|下载渠道|发布版|金丝雀版|
|-|-|-|
|Github Action 实时构建|[获取](https://github.com/vtbmusic/VtuberMusic-UWP/actions)|[获取](https://github.com/vtbmusic/VtuberMusic-UWP/actions)|
|蓝奏云|[获取 (密码:fkrd)](https://misaka-l.lanzous.com/b00o2ouja)|-|
|VtuberMusic OneDrive|[获取](https://t6q-my.sharepoint.com/:f:/g/personal/misaka-l_vtbmusic_icu/EsYtbLE0b9BHlqr5JGmyKRMBhTPPiJDg8lQJ79zJ74Jleg?e=0zLSBa)|-|
|微软商店|-|-|

## 开发相关
### 环境配置
- 操作系统: Windows 11 22000 或更高版本
- IDE: Microsoft Visual Studio 2019 或更新版本 (推荐 Microsoft Visual Studio 2022)
### 注意事项
- 贯穿整个应用生命周期的类都应添加到 App 类
- 设置分为本地设置和漫游设置 `App.LocalSettings` / `App.RoamingSettings`
- 显示 ContentDialog 时，请使用 `App.ContentDialogManager.ShowAsync()`，防止同时弹出多个 Content Dialog 导致出现异常
- Page 控制导航优先使用 `this.Frame.Navigate()`
### 项目目录结构
```
- VtuberMusic-UWP
├─Assets # 存储应用所有的资源文件，如图片 / 图标
├─Components # 控件
│  ├─Account # 账号控件 例: AccountPanel
│  ├─Collections # 合集控件 例: MusicDataList
│  ├─Comment # 评论控件
│  ├─DataItem # 数据项目 例: MusicCardItem
│  ├─Dialog # ContentDialog
│  ├─Lyric # 歌词控件
│  ├─Main # 主要控件 例: TopPanel
│  └─Player # 播放器相关控件 例: MainPlayer, PlayList
├─Models # 实体类
│  ├─DebugCommand # Debug 命令
│  ├─Lyric # 歌词
│  ├─Main # App 核心
│  └─VtuberMusic # VtuberMusic Api 数据实体类
├─Pages # 页面
│  ├─SearchPage # 搜索页面 NavgationView 使用的 Page
│  └─SetupPage # 初始设置页面
├─Service # 服务类 例: MusicClient, AccountService
├─Template # 控件 Template / Style
├─Theme # Dark / Light 颜色模式资源
└─Tools # 工具类
```