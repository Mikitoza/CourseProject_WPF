using CourseProject.Commands;
using CourseProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProject.ViewModels
{
    class SearchPageViewModel : ViewModelBase
    {
        private readonly USERS user;
        private readonly MusicService context = new MusicService();
        private MainWindowViewModel _mainWindowViewModel;
        public MainWindowViewModel MainWindowViewModel
        {
            get => _mainWindowViewModel;
            set => Set(ref _mainWindowViewModel, value);
        }
        private ALBUMS _selectAlbum;
        public ALBUMS selectAlbum
        {
            get => _selectAlbum;
            set => Set(ref _selectAlbum, value);
        }
        private TRACKS _selectTrack;
        public TRACKS selectTrack
        {
            get => _selectTrack;
            set => Set(ref _selectTrack, value);
        }
        private List<ALBUMS> _albums;
        public List<ALBUMS> albums
        {
            get => _albums;
            set => Set(ref _albums, value);
        }
        private List<TRACKS> _tracks;
        public List<TRACKS> tracks
        {
            get => _tracks;
            set => Set(ref _tracks, value);
        }
        public ICommand ToAlbumCommand { get; }
        private bool CanToAlbumCommandExecute(object p) => selectAlbum != null;
        private void OnToAlbumCommandExecuted(object p)
        {
            MainWindowViewModel.selectedVM = new AlbumPageViewModel(MainWindowViewModel, selectAlbum);
        }
        public ICommand AddTrackToUserTracksCommand { get; }
        private bool CanAddTrackToUserTracksCommandExecute(object p) => !context.USERS.Find(user.id_user).TRACKS1.Contains(selectTrack) & selectTrack != null;
        private void OnAAddTrackToUserTracksCommandExecuted(object p)
        {
            context.USERS.Find(user.id_user).TRACKS1.Add(selectTrack);
            context.SaveChanges();
        }
        public ICommand DeleteTrackToUserTracksCommand { get; }
        private bool CanDeleteTrackToUserTracksCommandExecute(object p) => context.USERS.Find(user.id_user).TRACKS1.Contains(selectTrack) & selectTrack != null;
        private void OnDeleteTrackToUserTracksCommandExecuted(object p)
        {
            context.USERS.Find(user.id_user).TRACKS1.Remove(selectTrack);
            context.SaveChanges();
        }
        public ICommand LoadCommand { get; }
        private bool CanLoadCommandExecute(object p) => selectTrack != null;
        private void OnLoadCommandExecuted(object p)
        {
            MainWindowViewModel.selectedTrack = selectTrack;
        }
        public SearchPageViewModel(List<TRACKS> tracks,List<ALBUMS> albums,MainWindowViewModel main,USERS user)
        {
            this.albums = albums;
            this.tracks = tracks;
            this.MainWindowViewModel = main;
            this.user = user;
            ToAlbumCommand = new LambdaCommand(OnToAlbumCommandExecuted,CanToAlbumCommandExecute);
            AddTrackToUserTracksCommand = new LambdaCommand(OnAAddTrackToUserTracksCommandExecuted, CanAddTrackToUserTracksCommandExecute);
            DeleteTrackToUserTracksCommand = new LambdaCommand(OnDeleteTrackToUserTracksCommandExecuted, CanDeleteTrackToUserTracksCommandExecute);
            LoadCommand = new LambdaCommand(OnLoadCommandExecuted, CanLoadCommandExecute);
        }
    }
}
