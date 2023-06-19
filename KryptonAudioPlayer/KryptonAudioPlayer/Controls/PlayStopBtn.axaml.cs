using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System.Windows.Input;

namespace KryptonAudioPlayer.Controls
{
    public partial class PlayStopBtn : UserControl
    {
        public PlayStopBtn()
        {
            InitializeComponent();
           
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        public static readonly StyledProperty<ICommand> CommandProperty =
      AvaloniaProperty.Register<PlayBtn, ICommand>(nameof(Command));

        public ICommand Command
        {
            get { return GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }

        }

        //private void PlayStop(object sender, RoutedEventArgs e)
        //{
        //    Button button = sender as Button;
        //    Image image = button.Find<Image>("PlayStopImage");

        //    if (button.Tag.ToString() == "stopped")
        //    {
        //        image.Source = new Bitmap("/Assets/Pause.png");
        //        button.Tag = "playing";
        //    }
        //    else
        //    {
        //        image.Source = new Bitmap("/Assets/Play.png");
        //        button.Tag = "stopped";
        //    }
        //}
    }
}
