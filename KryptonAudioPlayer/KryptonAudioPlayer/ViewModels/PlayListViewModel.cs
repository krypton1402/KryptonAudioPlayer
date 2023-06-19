using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using KryptonAudioPlayer.Models;
using NAudio.Wave;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Input;
using TagLib;
using TagLib.Riff;

namespace KryptonAudioPlayer.ViewModels
{
    public class PlayListViewModel : ReactiveObject
    {
        AudioFile pl = new AudioFile();

        // We need a public accessor to cl
        public AudioFile PL => pl;
        int selectedTrack;
        public int SelectedTrack
        {
            get => selectedTrack;
            set => this.RaiseAndSetIfChanged(ref selectedTrack, value);
        }

        string currentTrackTitle;
        public string CurrentTrackTitle
        {
            get => currentTrackTitle;
            set => this.RaiseAndSetIfChanged(ref currentTrackTitle, value);
        }

        string currentTrackArtist;
        public string CurrentTrackArtist
        {
            get => currentTrackArtist;
            set => this.RaiseAndSetIfChanged(ref currentTrackArtist, value);
        }

        Bitmap currentTrackCover;
        public Bitmap CurrentTrackCover
        {
            get => currentTrackCover;
            set => this.RaiseAndSetIfChanged(ref currentTrackCover, value);
        }

        ObservableCollection<AudioFile> tracks;
        public PlayListViewModel()
        {
            
            PlayMusicCommand = ReactiveCommand.Create(PlayMusic);
            PrevTrackCommand = ReactiveCommand.Create(PrevTrack);
            NextTrackCommand = ReactiveCommand.Create(NextTrack);
            tracks = new ObservableCollection<AudioFile>();
            //Реализация открытия проводника и добавления треков


            AddTracksCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var openFileDialog = new OpenFileDialog
                {
                    AllowMultiple = true,
                    Filters = new List<FileDialogFilter>
                    {
                        new FileDialogFilter { Name = "Audio files", Extensions = { "mp3", "wav" } }
                    }
                };

                var result = await openFileDialog.ShowAsync(GetMainWindow());


                if (result != null && result.Length > 0)
                {
                    foreach (var trackName in result)
                    {
                        //получаем путь
                        string filePath = Path.GetFullPath(trackName);
                        Bitmap coverImage = null;
                        //вытягирваем данные
                        TagLib.File file = TagLib.File.Create(filePath);
                        string title = file.Tag.Title;
                        string artist = file.Tag.FirstPerformer;
                        IPicture coverPicture = file.Tag.Pictures.FirstOrDefault();
                        if (coverPicture != null)
                        {
                            using MemoryStream stream = new MemoryStream(coverPicture.Data.Data);
                            Bitmap coverBitmap = new Bitmap(stream);
                            coverImage = coverBitmap;
                        }
                        var duration = file.Properties.Duration.TotalSeconds;
                        var minutes = (int)duration / 60;
                        var seconds = (int)duration % 60;

                        var newTrack = new AudioFile { Artist = artist, Title = title, FilePath = filePath, CoverImage = coverImage, Duration = $"{minutes}:{seconds:00}" };
                        if (!tracks.Contains(newTrack))
                        {
                            tracks.Add(newTrack);
                        }



                    }
                }
            });
            waveOut = new WaveOutEvent();
            
        }
        public ICommand PlayMusicCommand { get; }
        public ICommand PrevTrackCommand { get; }
        public ICommand NextTrackCommand { get; }
        public ICommand AddTracksCommand { get; }
        //Свойство для отображения списка на экране
        public ObservableCollection<AudioFile> Playlist
        {
            get => tracks;
            set => this.RaiseAndSetIfChanged(ref tracks, value);
        }


        private WaveOutEvent waveOut;
        private AudioFileReader audioFileReader;
        public bool isTrackPlay = false;
        public bool isTrackChanged = false;
        int saveTrack;

        private void PlayMusic()
        {

            if (SelectedTrack != null && tracks.Count > 0)
            {
                if (audioFileReader == null) //Создание объекта для вывода если его нет
                {
                    audioFileReader = new AudioFileReader(Playlist[SelectedTrack].FilePath);
                    waveOut.Init(audioFileReader);
                    waveOut.Play();
                    CurrentTrackTitle = Playlist[SelectedTrack].Title;
                    CurrentTrackArtist = Playlist[SelectedTrack].Artist;
                    CurrentTrackCover = Playlist[SelectedTrack].CoverImage;
                }
                else //Если он есть то проверяем
                {
                    if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        if (saveTrack == SelectedTrack) //Если текущий трек равен выбранному пользователем, то ставим паузу
                        {
                            waveOut.Pause();
                        }
                        else //Если нет, то реинициализируем трек
                        {

                            saveTrack = SelectedTrack;
                            ReinitializeTrack();
                            
                        }
                    }
                    else
                    {
                        if (saveTrack == SelectedTrack) //Если текущий трек равен выбранному пользователем, то Запускаем проигрывание
                        {
                            waveOut.Play();
                        }
                        else //Если нет, то реинициализируем трек
                        {
                            
                            saveTrack = SelectedTrack;
                            ReinitializeTrack();

                        }
                    }
                }
                saveTrack = SelectedTrack;
                //waveOut.PlaybackStopped += OnPlaybackStopped; // подписываемся на событие окончания проигрывания 
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    ChangeIcon = true;
                }
                else
                {
                    ChangeIcon = false;
                }
            }

        }
        private bool flag=false;
        private void ReinitializeTrack()
        {
            flag = true;
            waveOut.Stop();

            waveOut.Dispose();
           
            audioFileReader = new AudioFileReader(Playlist[SelectedTrack].FilePath);
            waveOut.Init(audioFileReader);
            CurrentTrackTitle = Playlist[SelectedTrack].Title;
            CurrentTrackArtist = Playlist[SelectedTrack].Artist;
            CurrentTrackCover = Playlist[SelectedTrack].CoverImage;
            PlayMusic();
            flag = false;
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if(flag==true)
            { return; }
            if (SelectedTrack < tracks.Count - 1)
            {
                SelectedTrack++; // переходим на следующий трек
                PlayMusic();
            }
            else
            {
                // Последний трек уже проигран, останавливаем проигрывание
                waveOut.Stop();
                isTrackPlay = false;
            }

        }

        public void RewindBackward()
        {
            if (audioFileReader != null)
            {
                var newPosition = audioFileReader.CurrentTime - TimeSpan.FromSeconds(10);
                if (newPosition.TotalSeconds < 0)
                {
                    newPosition = TimeSpan.Zero;
                }
                audioFileReader.Seek((long)newPosition.TotalMilliseconds, SeekOrigin.Begin);
            }
        }




        public void PrevTrack()
        {
            if (SelectedTrack > 0)
            {
                SelectedTrack--;
                ReinitializeTrack();
                
            }
            else if (SelectedTrack == 0)
            {
                SelectedTrack = Playlist.Count - 1;
                ReinitializeTrack();
            }
        }
        public void NextTrack()
        {
            if (SelectedTrack < tracks.Count - 1)
            {
                SelectedTrack++;
                ReinitializeTrack();

            }
            else
            {
                SelectedTrack = 1;
                ReinitializeTrack();
            }
        }

        private Window GetMainWindow()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            else if (Application.Current.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                return (Window)singleView.MainView;
            }
            else
            {
                return null;
            }
        }

        public bool changeIcon = false;
        public bool ChangeIcon
        {
            get => changeIcon;
            set => this.RaiseAndSetIfChanged(ref changeIcon, value);
        }
    }

}
