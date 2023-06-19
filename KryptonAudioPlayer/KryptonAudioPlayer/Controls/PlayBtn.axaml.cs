using Avalonia;
using Avalonia.Controls.Primitives;
using System.Windows.Input;

namespace KryptonAudioPlayer.Controls
{
    public class PlayBtn : TemplatedControl
    {
        public static readonly StyledProperty<ICommand> CommandProperty =
       AvaloniaProperty.Register<PlayBtn, ICommand>(nameof(Command));
       
        public ICommand Command
        {
            get { return GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }

        }

    }
}
