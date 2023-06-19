using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using KryptonAudioPlayer.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KryptonAudioPlayer.Controls
{
    public class ListTrackForwardBtn : TemplatedControl
    {
        private bool _isPressed = false;
        private Point _positionInBlock;

        private const int LongPressDuration = 500; // ������������ �������� ������� � �������������
        private CancellationTokenSource _rewindCancelTokenSource; // ����� ��� ������ ��������� �����
        private DateTime _pressStartTime; // ����� ������ �������

        private DispatcherTimer timer = new DispatcherTimer();
        private PlayListViewModel pl = new PlayListViewModel();
        public ListTrackForwardBtn()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel, true);
        }


        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            _pressStartTime = DateTime.Now;
            _rewindCancelTokenSource = new CancellationTokenSource();
            Task.Run(async () => await RewindTrack(_rewindCancelTokenSource.Token)); // ������ ������ ��������� �����

        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            _isPressed = false;
            if ((DateTime.Now - _pressStartTime).TotalMilliseconds >= LongPressDuration)
            {
                // ������� ������� - ��������� ��������� �����
                _rewindCancelTokenSource?.Cancel();
            }
            else
            {
                pl.PrevTrack();
            }

        }
        private async Task RewindTrack(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1)); // �������� �� 1 �������
                if (!cancellationToken.IsCancellationRequested)
                {
                    pl.RewindBackward();
                }
            }
        }
        public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ListTrackForwardBtn, ICommand>(nameof(Command));
        public ICommand Command
        {
            get { return GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }

        }
    }
}
