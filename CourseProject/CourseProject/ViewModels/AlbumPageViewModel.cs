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
using System.Net;

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
        private ALBUMS _album;
        private bool _IsSinger;
        public bool IsSinger
        {
            get => _IsSinger;
            set => Set(ref _IsSinger, value);
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
        public ICommand AddTrackToUserTracksCommand { get; }
        private bool CanAddTrackToUserTracksCommandExecute(object p) => !context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Contains(MainWindowViewModel.selectedTrack) & MainWindowViewModel.selectedTrack != null;
        private void OnAAddTrackToUserTracksCommandExecuted(object p)
        {
            context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Add(MainWindowViewModel.selectedTrack);
            context.SaveChanges();
        }
        public ICommand DeleteTrackToUserTracksCommand { get; }
        private bool CanDeleteTrackToUserTracksCommandExecute(object p) => context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Contains(MainWindowViewModel.selectedTrack) & MainWindowViewModel.selectedTrack != null;
        private void OnDeleteTrackToUserTracksCommandExecuted(object p)
        {
            context.USERS.Find(MainWindowViewModel.user.id_user).TRACKS1.Remove(MainWindowViewModel.selectedTrack);
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
            AddTrackToUserTracksCommand = new LambdaCommand(OnAAddTrackToUserTracksCommandExecuted, CanAddTrackToUserTracksCommandExecute);
            DeleteTrackToUserTracksCommand = new LambdaCommand(OnDeleteTrackToUserTracksCommandExecuted, CanDeleteTrackToUserTracksCommandExecute);
        }
    }
}
