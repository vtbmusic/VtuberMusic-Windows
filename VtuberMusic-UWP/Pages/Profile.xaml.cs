using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace VtuberMusic_UWP.Pages {
    /// <summary>
    /// 账户资料页
    /// </summary>
    public sealed partial class Profile : Page {
        private Models.VtuberMusic.Profile profile;
        private bool cachePage = false;
        private object _albumItem;
        private string albumPageType;

        public Profile() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if (e.Parameter != null && e.Parameter.GetType() == typeof(Models.VtuberMusic.Profile)) {
                profile = e.Parameter as Models.VtuberMusic.Profile;
                this.load();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            if (!this.cachePage) this.NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private async void load() {
            this.Nickname.Text = profile.nickname;
            this.LevelText.Text = $"Lv.{ profile.level.ToString() }";
            
            if (this.profile.nextexperience.GetValueOrDefault() != 0) {
                this.LevelExp.Maximum = this.profile.nextexperience.GetValueOrDefault();
            } else {
                this.LevelExp.Minimum = this.profile.experience.GetValueOrDefault();
            }

            this.LevelExp.Value = profile.experience.GetValueOrDefault();
            this.Avatar.ProfilePicture = new BitmapImage(new Uri(profile.avatarUrl));

            var createAlbum = await App.Client.GetMyCreatePlayList(profile.userId);
            var favouriteAlbum = await App.Client.GetMyFavouritePlayList(profile.userId);
            var recentPlay = await App.Client.GetMusicRecordList(profile.userId);
            var likeMusicAlbum = await App.Client.GetLikeMusicSong(profile.userId);

            this.CreateAlbumDataView.ItemsSource = createAlbum.Data;
            this.CreateAlbumCount.Text = "共 " + createAlbum.Data.Length + " 项";

            this.FavourtiteAlbumDataView.ItemsSource = favouriteAlbum.Data;
            this.FavouriteAlbumCount.Text = "共 " + favouriteAlbum.Data.Length + " 项";

            this.LikeMusicAlbumDataView.ItemsSource = new Models.VtuberMusic.Album[] { likeMusicAlbum.Data.playlist };
            this.LikeMusicTitle.Text = this.profile.nickname + " 喜欢的音乐";

            this.RecntPlay.ItemsSource = recentPlay.Data;
        }

        private async void CreateAlbumDataView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            if (this.albumPageType != "create") return;
            this.cachePage = false;

            this.CreateAlbumDataView.ScrollIntoView(this._albumItem);
            this.CreateAlbumDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null) {
                await this.CreateAlbumDataView.TryStartConnectedAnimationAsync(animation, this._albumItem, "CoverImgBorder");
            }
        }

        private void CreateAlbumDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this.cachePage = true;

            this.albumPageType = "create";
            this._albumItem = ( (GridViewItem)this.CreateAlbumDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.CreateAlbumDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                this._albumItem,
                "CoverImgBorder");

            this.Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void FavoutiteAlbumDataView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            if (this.albumPageType != "fav") return;
            this.cachePage = false;

            this.FavourtiteAlbumDataView.ScrollIntoView(this._albumItem);
            this.FavourtiteAlbumDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null) {
                await this.FavourtiteAlbumDataView.TryStartConnectedAnimationAsync(animation, this._albumItem, "CoverImgBorder");
            }
        }

        private void FavoutiteAlbumDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this.cachePage = true;

            this.albumPageType = "fav";
            this._albumItem = ( (GridViewItem)this.FavourtiteAlbumDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.FavourtiteAlbumDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                this._albumItem,
                "CoverImgBorder");

            this.Frame.Navigate(typeof(Album), e.ClickedItem, new DrillInNavigationTransitionInfo());
        }

        private async void LikeMusicAlbumDataView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            if (this.albumPageType != "like") return;
            this.cachePage = false;

            this.LikeMusicAlbumDataView.ScrollIntoView(this._albumItem);
            this.LikeMusicAlbumDataView.UpdateLayout();

            ConnectedAnimation animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackConnectedAnimation");
            if (animation != null) {
                await this.LikeMusicAlbumDataView.TryStartConnectedAnimationAsync(animation, this._albumItem, "CoverImgBorder");
            }
        }

        private void LikeMusicAlbumDataView_ItemClick(object sender, ItemClickEventArgs e) {
            this.cachePage = true;

            this.albumPageType = "like";
            this._albumItem = ( (GridViewItem)this.LikeMusicAlbumDataView.ContainerFromItem(e.ClickedItem) ).Content;
            var animation = this.LikeMusicAlbumDataView.PrepareConnectedAnimation("ForwardConnectedAnimation",
                this._albumItem,
                "CoverImgBorder");

            this.Frame.Navigate(typeof(Album), new AlbumPageArgs { Album = e.ClickedItem as Models.VtuberMusic.Album, IsLikeMusic = true }, new DrillInNavigationTransitionInfo());
        }
    }
}
