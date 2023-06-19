using Avalonia.Input;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace KryptonAudioPlayer.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        PlayListViewModel pl = new PlayListViewModel();

        // We need a public accessor to cl
        public PlayListViewModel PL => pl;
        public MainWindowViewModel()
        {
            OpacityCommand = ReactiveCommand.Create(ShowList);

        }
        public ICommand OpacityCommand { get; }
        private double opacityPanel = 1;
        public double OpacityPanel
        {
            get => opacityPanel;
            set => this.RaiseAndSetIfChanged(ref opacityPanel, value);
        }
        
        public void ShowList()
        {
            if (opacityPanel == 0) 
            {
                OpacityPanel = 1;
            }
            else
            {
                OpacityPanel = 0;
            }
        }






    }





}
