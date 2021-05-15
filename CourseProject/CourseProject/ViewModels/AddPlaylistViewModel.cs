using CourseProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject.Commands;

namespace CourseProject.ViewModels
{
    class AddPlaylistViewModel : ViewModelBase
    {
        private readonly MusicService context = new MusicService();
        private readonly MainWindowViewModel main;
        private string _Search;
        public string Search
        {
            get => _Search;
            set => Set(ref _Search, value);
        }
        private List<TRACKS> _Tracklist= new MusicService().TRACKS.ToList();
        public List<TRACKS> TrackList
        {
            get => _Tracklist;
            set => Set(ref _Tracklist, value);
        }
        private List<TRACKS> _PlaylistTracklist;
        public List<TRACKS> PlaylistTracklist
        {
            get => _PlaylistTracklist;
            set => Set(ref _PlaylistTracklist, value);
        }
        private PLAYLISTS _CurrentPlaylist;
        public PLAYLISTS CurrentPlaylist
        {
            get => _CurrentPlaylist;
            set => Set(ref _CurrentPlaylist, value);
        }
        private TRACKS _selectedTrack;
        public TRACKS selectedTrack
        {
            get => _selectedTrack;
            set => Set(ref _selectedTrack, value);
        }
        private TRACKS _selectedPlaylistTrack;
        public TRACKS selectedPlaylistTrack
        {
            get => _selectedPlaylistTrack;
            set => Set(ref _selectedPlaylistTrack, value);
        }
        private string _playlistName;
        public string playlistName
        {
            get => _playlistName;
            set => Set(ref _playlistName, value);
        }
        public ICommand SearchTracks { get; }
        public bool CanSearchTracksCommandExecute(object p) => Search?.Length > 3;
        public void OnSearchTrackCommandExecuted(object p)
        {
            TrackList = context.TRACKS.Where(tr => tr.track_name.Contains(Search)).ToList();
        }
        public ICommand AddPlaylistCommand { get; }
        public bool CanAddPlaylistCommandExecute(object p) => playlistName?.Length > 3 && CurrentPlaylist ==null;
        public void OnAddPlaylistCommandExecuted(object p)
        {
            PLAYLISTS playlist = new PLAYLISTS(playlistName,main.user.id_user);
            context.PLAYLISTS.Add(playlist);
            context.SaveChanges();
            CurrentPlaylist = context.PLAYLISTS.First(pl => pl.playlist_name == playlistName && pl.id_user == main.user.id_user);
            RestoreForm();
        }

        public ICommand AddTrackToPlaylistCommand { get; }
        public bool CanAddTrackToPlaylistCommandExecute(object p) => selectedTrack != null && CurrentPlaylist != null;
        public void OnAddTrackToPlaylistCommandExecuted(object p)
        {
            PLAYLISTS play = context.PLAYLISTS.First(pl => pl.playlist_name == CurrentPlaylist.playlist_name);
            context.PLAYLISTS.Find(play).TRACKS.Add(selectedTrack);
            context.SaveChanges();
            PlaylistTracklist = context.PLAYLISTS.Find(CurrentPlaylist).TRACKS.ToList();
        }
        public ICommand DeleteTrackToPlaylistCommand { get; }
        public bool CanDeleteTrackToPlaylistCommandExecute(object p) => selectedPlaylistTrack != null && CurrentPlaylist != null;
        public void OnDeleteTrackToPlaylistCommandExecuted(object p)
        {
            context.PLAYLISTS.Find(CurrentPlaylist).TRACKS.Add(selectedTrack);
            context.SaveChanges();
            PlaylistTracklist = context.PLAYLISTS.Find(CurrentPlaylist).TRACKS.ToList();
        }
        public AddPlaylistViewModel(MainWindowViewModel main)
        {
            this.main = main;
            SearchTracks = new LambdaCommand(OnSearchTrackCommandExecuted, CanSearchTracksCommandExecute);
            AddPlaylistCommand = new LambdaCommand(OnAddPlaylistCommandExecuted, CanAddPlaylistCommandExecute);
            AddTrackToPlaylistCommand = new LambdaCommand(OnAddTrackToPlaylistCommandExecuted, CanAddTrackToPlaylistCommandExecute);
            DeleteTrackToPlaylistCommand = new LambdaCommand(OnDeleteTrackToPlaylistCommandExecuted, CanDeleteTrackToPlaylistCommandExecute);
        }
        public void RestoreForm()
        {
            Search = "";
            playlistName = "";
            selectedTrack = null;
            selectedPlaylistTrack = null;
            TrackList = context.TRACKS.ToList();
    }
    }
}
