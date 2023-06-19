using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KryptonAudioPlayer.Views
{
    public partial class PlayBar : UserControl
    {
        public PlayBar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
