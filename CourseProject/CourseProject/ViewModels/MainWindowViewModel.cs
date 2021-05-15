using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CourseProject.Commands;
using CourseProject.DataModels;
using CourseProject.DataModels.ENUM;

namespace CourseProject.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly string _myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private readonly MusicService musicService = new MusicService();
        private bool IsEng = true;
        private bool IsPlay = false;
        private double _sliderValue;
        private string track_name;
        private string _Search;
        public string Search
        {
            get => _Search;
            set => Set(ref _Search, value);
        }
        private TimeSpan _TimeSpan;
        public TimeSpan TimeSpan
        {
            get => _TimeSpan;
            set => Set(ref _TimeSpan, value);
        }
        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                if (_sliderValue != SliderValue)
                {
                    _sliderValue = SliderValue;
                    mediaPlayer.Position = TimeSpan.FromMilliseconds(SliderValue);

                    RaisePropertyChanged("SliderValue");
                }
            }
        }
        private double _SliderMaximum;
        public double SliderMaximum
        {
            get => _SliderMaximum;
            set => Set(ref _SliderMaximum, value);
        }
        private string _ButtonPlay = "Play";
        public string ButtonPlay
        {
            get => _ButtonPlay;
            set => Set(ref _ButtonPlay, value);
        }
        private USERS _user;
        public USERS user
        {
            get => _user;
            set => Set(ref _user, value);
        }
        private string _imgPath;
        public string imgPath
        {
            get => _imgPath;
            set => Set(ref _imgPath, value);
        }
        private string _AlbumName;
        public string AlbumName
        {
            get => _AlbumName;
            set => Set(ref _AlbumName, value);
        }
        private bool _IsSinger;
        public bool IsSinger
        {
            get => _IsSinger;
            set => Set(ref _IsSinger, value);
        }
        private bool _IsAdmin;
        public bool IsAdmin
        {
            get => _IsAdmin;
            set => Set(ref _IsAdmin, value);
        }
        private TRACKS _selectedTrack;
        public TRACKS selectedTrack
        {
            get => _selectedTrack;
            set
            {
                _selectedTrack = value;
                Stoping();
                RaisePropertyChanged("selectedTrack");
            }
        }
        private PLAYLISTS _selectedPlaylist;
        public PLAYLISTS selectedPlaylist
        {
            get => _selectedPlaylist;
            set => Set(ref _selectedPlaylist, value);
        }
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        public MediaPlayer mediaPlayer
        {
            get => _mediaPlayer;
            set => Set(ref _mediaPlayer, value);
        }
        private string _Title;
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        private ViewModelBase _selectedVM;
        public ViewModelBase selectedVM
        {
            get => _selectedVM;
            set => Set(ref _selectedVM, value);
        }
        private List<PLAYLISTS> _playlists;
        public List<PLAYLISTS> playlists
        {
            get => _playlists;
            set => Set(ref _playlists, value);
        }
        void Stoping()
        {
            mediaPlayer.Stop();
            ButtonPlay = "Play";
        }
        #region Команды
        #region CloseAppComand
        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        #endregion
        #region SwitchView
        public ICommand SwitchViewCommand { get; }
        private bool CanSwitchViewCommandExecute(object p) => true;
        private void OnSwitchViewCommandExecuted(object p)
        {
            switch (p.ToString())
            {
                case "SingerPage":
                    selectedVM = new SingerPageViewModel(user,this);
                    break;
                case "AdminPage":
                    selectedVM = new AdminPageViewModel();
                    break;
                case "AddAlbumPage":
                    selectedVM = new AddAlbumViewModel(user);
                    break;
                case "MainPage":
                    selectedVM = new MainPageViewModel(user,this);
                    break;
                case "ProfilePage":
                    selectedVM = new ProfilePageViewModel(user,this);
                    break;
            }
        }
        #endregion
        #region SwitchLanguage
        public ICommand SwitchLanguageCommand { get; }
        private bool CanSwitchLanguageCommandExecute(object p) => true;
        private void OnSwitchLanguageCommandExecuted(object p)
        {
            if (IsEng)
            {
                App.Language = new CultureInfo("ru-RU");
                IsEng = false;
            }
            else
            {
                App.Language = new CultureInfo("en-US");
                IsEng = true;
            }
        }
        #endregion
        #region AddPlaylist
        public ICommand AddPlaylistCommand { get; }
        private bool CanAddPlaylistCommandExecute(object p) => true;
        private void OnAddPlaylistCommandExecuted(object p)
        {
            selectedVM = new AddPlaylistViewModel(this);
        }
        #endregion
        #region Play
        public ICommand PlayTraclCommand { get; }
        private bool CanPlayTraclCommandExecute(object p) => !(selectedTrack == null);
        private void OnPlayTraclCommandExecuted(object p)
        {
            if (track_name != selectedTrack.track_name)
            {
                MessageBox.Show(selectedTrack.album_id.ToString());
                MessageBox.Show(selectedTrack.track_name.ToString());
                ButtonPlay = "Play";
                mediaPlayer.Stop();
                mediaPlayer.Open(new Uri($"http://localhost:3000/albums/{selectedTrack.album_id}/{selectedTrack.track_name}.mp3"));
                mediaPlayer.Play();
                track_name = selectedTrack.track_name;
                IsPlay = true;
                ButtonPlay = "Pause";
            }
            else
            {
                if (IsPlay)
                {
                    mediaPlayer.Pause();
                    IsPlay = false;
                    ButtonPlay = "Play";
                }
                else
                {
                    mediaPlayer.Play();
                    IsPlay = true;
                    ButtonPlay = "Pause";
                }
            }
        }
        #endregion
        #region SearchCommand
        public ICommand SearchCommand { get; }
        private bool CanSearchCommandExecute(object p) => Search?.Length >= 3;
        private void OnSearchCommandExecuted(object p)
        {
            List<TRACKS> tracks = musicService.TRACKS.Where(tr => tr.track_name.Contains(Search)).ToList();
            List<ALBUMS> albums = musicService.ALBUMS.Where(al => al.albums_name.Contains(Search)).ToList();
            selectedVM = new SearchPageViewModel(tracks, albums,this,user);
        }

        #endregion
        #endregion
        public MainWindowViewModel(USERS user)
        {
            this.user = user;
            IsSinger = user.user_role == (int)UserRole.Singer;
            IsAdmin = user.user_role == (int)UserRole.Admin;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            selectedVM = new MainPageViewModel(user,this);
            playlists = musicService.PLAYLISTS.Where(pl => pl.id_user == user.id_user).ToList();
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted, CanSwitchViewCommandExecute);
            PlayTraclCommand = new LambdaCommand(OnPlayTraclCommandExecuted, CanPlayTraclCommandExecute);
            SearchCommand = new LambdaCommand(OnSearchCommandExecuted, CanSearchCommandExecute);
            SwitchLanguageCommand = new LambdaCommand(OnSwitchLanguageCommandExecuted, CanSwitchLanguageCommandExecute);
            AddPlaylistCommand = new LambdaCommand(OnAddPlaylistCommandExecuted, CanAddPlaylistCommandExecute);
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            SliderMaximum = mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            TimeSpan = mediaPlayer.NaturalDuration.TimeSpan;
        }
    }
}
