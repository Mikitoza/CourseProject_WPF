using CourseProject.Commands;
using CourseProject.DataModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProject.ViewModels
{
    class ChangeAlbumViewModel : ViewModelBase
    {
        private readonly USERS user;
        private readonly WebClient myWebClient = new WebClient();
        private MainWindowViewModel _mainWindowViewModel;
        public MainWindowViewModel mainWindowViewModel
        {
            get => _mainWindowViewModel;
            set => Set(ref _mainWindowViewModel, value);
        }
        private string _albumName;
        public string albumName
        {
            get => _albumName;
            set => Set(ref _albumName, value);
        }
        private ALBUMS _album;
        public ALBUMS album
        {
            get => _album;
            set => Set(ref _album, value);
        }
        private readonly string _myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private readonly MusicService context = new MusicService();
        private string _Name;
        private string _Singer;
        private ObservableCollection<TRACKS> _tracks = new ObservableCollection<TRACKS>();
        private int _tracksquat;
        public int tracksquat
        {
            get => _tracksquat;
            set => Set(ref _tracksquat, value);
        }
        private string _mp3Path;
        private bool _IsSinger;
        public bool IsSinger
        {
            get => _IsSinger;
            set => Set(ref _IsSinger, value);
        }
        private TRACKS _selectTrack;
        public TRACKS selectTrack
        {
            get => _selectTrack;
            set => Set(ref _selectTrack, value);
        }
        private bool _dialog = false;
        public bool dialog
        {
            get => _dialog;
            set => Set(ref _dialog, value);
        }
        public string mp3Path
        {
            get => _mp3Path;
            set => Set(ref _mp3Path, value);
        }
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }
        public MainWindowViewModel MainWindowViewModel
        {
            get => _mainWindowViewModel;
            set => Set(ref _mainWindowViewModel, value);
        }
        public string Singer
        {
            get => _Singer;
            set => Set(ref _Singer, value);
        }
        public ObservableCollection<TRACKS> tracks
        {
            get => _tracks;
            set => Set(ref _tracks, value);
        }
        private void RestoreForm()
        {
            tracks = new ObservableCollection<TRACKS>();
            var tracks1 = context.TRACKS.Where(t => t.album_id == album.album_id);
            foreach (TRACKS track in tracks1)
            {
                tracks.Add(track);
            }
            Name = "";
            _mp3Path = "";
        }
        public ICommand AddTrackCommand { get; }
        private bool CanAddTrackExecute(object p) => Name != null && _mp3Path?.Length >0;
        private void OnAddTrackCommandExecuted(object p)
        {
            TRACKS track = new TRACKS(Name, (int)album.id_user, album.album_id, (int)album.genre_id);
            context.TRACKS.Add(track);
            context.SaveChanges();
            byte[] responseArray = myWebClient.UploadFile($"http://localhost:3000/upload/{track.album_id}/{track.track_name}", _mp3Path);
            RestoreForm();
        }
        public ICommand SelectMp3PathCommand { get; }
        private bool CanSelectMp3PathCommandExecute(object p) => _mainWindowViewModel.user.user_role == 1;
        private void OnSelectMp3PathCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Музыка|*.mp3;";
            if (openFileDialog.ShowDialog() == true)
            {
                _mp3Path = openFileDialog.FileName;
            }
        }
        public ICommand CloseDialogCommand { get; }
        private bool CanCloseDialogCommandExecute(object p) => true;
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;
        public ICommand PlayTrackCommand { get; }
        private bool CanPlayTrackCommandExecute(object p) => !(selectTrack == null);
        private void OnPlayTrackCommandExecuted(object p)
        {

            _mainWindowViewModel.selectedTrack = selectTrack;
        }
        public ICommand DeleteTrackCommand { get; }
        private bool CanDeleteTrackCommandExecute(object p) => !(selectTrack == null);
        private void OnDeleteTrackCommandExecuted(object p)
        {
            context.TRACKS.Remove(selectTrack);
            context.SaveChanges();
            RestoreForm();
        }
        public ICommand ChangeAlbumCommand { get; }
        public bool CanChangeAlbumCommandExecute(object p) => albumName != null;
        public void OnChangeAlbumCommandExecuted(object p)
        {
            var ChangeAlbum = context.ALBUMS.Where(alb => alb.album_id == album.album_id).FirstOrDefault();
            if (ChangeAlbum != null)
            {
                ChangeAlbum.albums_name = albumName;
                context.SaveChanges();
                album.albums_name = albumName;
            }
            albumName = null;
        }
        public ChangeAlbumViewModel(MainWindowViewModel main,ALBUMS alb)
        {
            this.mainWindowViewModel = main;
            this.album = alb;
            user = main.user;
            Singer = context.USERS.First(u => u.id_user == album.id_user).user_login;
            var tracks = context.TRACKS.Where(t => t.album_id == album.album_id);
            foreach (TRACKS track in tracks)
            {
                _tracks.Add(track);
            }
            tracksquat = tracks.Count() * 50;
            IsSinger = user.user_role == 1;
            AddTrackCommand = new LambdaCommand(OnAddTrackCommandExecuted, CanAddTrackExecute);
            SelectMp3PathCommand = new LambdaCommand(OnSelectMp3PathCommandExecuted, CanSelectMp3PathCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted, CanCloseDialogCommandExecute);
            PlayTrackCommand = new LambdaCommand(OnPlayTrackCommandExecuted, CanPlayTrackCommandExecute);
            DeleteTrackCommand = new LambdaCommand(OnDeleteTrackCommandExecuted, CanDeleteTrackCommandExecute);
            ChangeAlbumCommand = new LambdaCommand(OnChangeAlbumCommandExecuted,CanChangeAlbumCommandExecute);
        }
    }
}
