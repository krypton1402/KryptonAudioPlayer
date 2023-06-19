using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KryptonAudioPlayer.Controls;
using KryptonAudioPlayer.ViewModels;
using System;
using Windows.UI.Xaml.Media.Animation;

namespace KryptonAudioPlayer.Views
{
    public partial class MainWindow : Window
    {
        private PlayListViewModel pl = new PlayListViewModel();
        public MainWindow()
        {
            PlayListViewModel pl = new PlayListViewModel();
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            var button = this.FindControl<PlaylistBtn>("Playlist");
            var spisok = this.FindControl<PlayListView>("Spisok");

            
        }
      

        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            pl.RewindBackward();
        }
    }
}
