using CourseProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using CourseProject.Commands;

namespace CourseProject.ViewModels
{
    class AlbumPageViewModel : ViewModelBase
    {
        private readonly USERS user;
        private readonly string _myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private readonly MusicService context = new MusicService();
        private string _Name;
        private MainWindowViewModel _mainWindowViewModel;
        private string _Singer;
        private ObservableCollection<TRACKS> _tracks = new ObservableCollection<TRACKS>();
        private int _tracksquat;
        public int tracksquat
        {
            get => _tracksquat;
            set => Set(ref _tracksquat, value);
        }
        private string _mp3Path;
        private ALBUMS _album;
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
        public ALBUMS album
        {
            get => _album;
            set => Set(ref _album, value);
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
        private bool CanAddTrackExecute(object p) => _mainWindowViewModel.user.user_role == 1;
        private void OnAddTrackCommandExecuted(object p)
        {
            TRACKS track = new TRACKS(Name,(int)album.id_user,album.album_id,(int)album.genre_id);
            context.TRACKS.Add(track);
            context.SaveChanges();
            File.Copy(_mp3Path, _myDocumentsPath + $@"\MusicService\{album.album_id}\{track.track_name}.mp3", true);
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
        public ICommand AddTrackToUserTracksCommand { get; }
        private bool CanAddTrackToUserTracksCommandExecute(object p) => !context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Contains(selectTrack) & selectTrack !=null;
        private void OnAAddTrackToUserTracksCommandExecuted(object p)
        {
            context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Add(selectTrack);
            context.SaveChanges();
        }
        public ICommand DeleteTrackToUserTracksCommand { get; }
        private bool CanDeleteTrackToUserTracksCommandExecute(object p) => context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Contains(selectTrack) & selectTrack != null;
        private void OnDeleteTrackToUserTracksCommandExecuted(object p)
        {
            context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Remove(selectTrack);
            context.SaveChanges();
        }
        public AlbumPageViewModel(MainWindowViewModel window, ALBUMS album)
        {
            _mainWindowViewModel = window;
            user = window.user;
            _album = album;
            Singer = context.USERS.First(u => u.id_user == album.id_user).user_nickname;
            var tracks = context.TRACKS.Where(t => t.album_id == album.album_id);
            foreach (TRACKS track in tracks)
            {
                _tracks.Add(track);
            }
            tracksquat = tracks.Count() * 50;
            IsSinger = user.user_role == 1;
            AddTrackCommand = new LambdaCommand(OnAddTrackCommandExecuted,CanAddTrackExecute);
            SelectMp3PathCommand = new LambdaCommand(OnSelectMp3PathCommandExecuted, CanSelectMp3PathCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted, CanCloseDialogCommandExecute);
            PlayTrackCommand = new LambdaCommand(OnPlayTrackCommandExecuted, CanPlayTrackCommandExecute);
            AddTrackToUserTracksCommand = new LambdaCommand(OnAAddTrackToUserTracksCommandExecuted, CanAddTrackToUserTracksCommandExecute);
            DeleteTrackToUserTracksCommand = new LambdaCommand(OnDeleteTrackToUserTracksCommandExecuted, CanDeleteTrackToUserTracksCommandExecute);
        }
    }
}
