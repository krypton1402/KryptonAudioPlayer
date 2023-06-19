using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KryptonAudioPlayer.Views
{
    public partial class PlayListView : UserControl
    {
        public PlayListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
