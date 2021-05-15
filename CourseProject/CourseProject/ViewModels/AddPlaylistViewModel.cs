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
        private List<TRACKS> _Tracklist= new List<TRACKS>();
        public List<TRACKS> Tracklist
        {
            get => _Tracklist;
            set => Set(ref _Tracklist, value);
        }
        public ICommand SearchTracks { get; }
        public bool CanSearchTracksCommandExecute(object p) => Search?.Length > 3;
        public void OnSearchTrackCommandExecuted(object p)
        {
            Tracklist = context.TRACKS.Where(tr => tr.track_name.Contains(Search)).ToList();
        }
        public AddPlaylistViewModel(MainWindowViewModel main)
        {
            this.main = main;
            SearchTracks = new LambdaCommand(OnSearchTrackCommandExecuted, CanSearchTracksCommandExecute);
        }
    }
}
