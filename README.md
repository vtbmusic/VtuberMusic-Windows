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
### 文件分类
- Assets - 存储应用所有的资源文件，如图片 / 图标
- Components - 存储控件
- Pages - 存储页面文件
- Service - 服务相关，如 Api 客户端和播放器
- Tools - 一些杂七杂八的工具类，如网络工具
- Theme - 不同配色下的主题资源，如暗模式和亮模式
- Template - 一些扩件样式
- Models - 实体类，也包括部分逻辑代码
### Api 部分
- 账号相关功能: Service/AccountService.cs
- 音乐相关功能: Service/MusicClient.cs
- Api 地址定义: Models/VtuberMusic/ApiUri.cs
- 网络请求: Tools/NetworkTool.cs
### 歌词部分
- 歌词显示: Pages/Playing.xaml 和 Pages/Playing.xaml.cs
- 解析: Tools/Lyric.cs