using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using VtuberMusic.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VtuberMusic.App.Controls.Comments;
public sealed partial class CommentItem : UserControl {
    public static DependencyProperty CommentProperty =
        DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentItem), new PropertyMetadata(null));

    public Comment Comment {
        get => (Comment)GetValue(CommentProperty);
        set => SetValue(CommentProperty, value);
    }

    public CommentItem() {
        this.InitializeComponent();
    }
}
