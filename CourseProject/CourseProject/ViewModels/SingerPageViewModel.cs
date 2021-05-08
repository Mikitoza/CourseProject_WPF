using CourseProject.Commands;
using CourseProject.DataModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading;

namespace CourseProject.ViewModels
{
    class SingerPageViewModel : ViewModelBase
    {
        private MainWindowViewModel _MainWindowViewModel;
        private readonly string _myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private readonly USERS user;
        private readonly MusicService context = new MusicService();
        private ALBUMS _selectAlbum;
        public ALBUMS selectAlbum
        {
            get => _selectAlbum;
            set => Set(ref _selectAlbum, value);
        }
        private string _Title;
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        public MainWindowViewModel MainWindowViewModel
        {
            get => _MainWindowViewModel;
            set => Set(ref _MainWindowViewModel, value);
        }
        private ObservableCollection<ALBUMS> _albumslist = new ObservableCollection<ALBUMS>();
        public ObservableCollection<ALBUMS> albumsList
        {
            get => _albumslist;
            set => Set(ref _albumslist, value);
        }
        private void RestoreForm()
        {
            albumsList = new ObservableCollection<ALBUMS>();
            var albums = context.ALBUMS;
            foreach (ALBUMS album in albums)
            {
                albumsList.Add(album);
            }
        }
        public ICommand AlbumPageCommand { get; }
        private bool CanAlbumPageCommandExecute(object p) => !(selectAlbum ==null);
        private void OnAlbumPageCommandExecuted(object p)
        {
            MainWindowViewModel.selectedVM = new AlbumPageViewModel(MainWindowViewModel,selectAlbum);
        }
        public ICommand DeleteAlbumCommand { get; }
        private bool CanDeleteAlbumCommandExecute(object p) => !(selectAlbum == null);
        private void OnDeleteAlbumCommandExecuted(object p)
        {
            if (MainWindowViewModel.selectedTrack != null)
            {
                if (MainWindowViewModel.selectedTrack.album_id == selectAlbum.album_id)
                {
                    MainWindowViewModel.selectedTrack = null;
                }
            }
            int album_id = selectAlbum.album_id;
            foreach(TRACKS track in context.TRACKS.Where(tr => tr.album_id == selectAlbum.album_id))
            {
                context.TRACKS.Remove(track);
                foreach(USERS user in context.USERS)
                {
                    user.TRACKS1.Remove(track);
                }
            }
            context.SaveChanges();
            context.ALBUMS.Remove(selectAlbum);
            context.SaveChanges();
            RestoreForm();
            try
            {
                File.Delete(_myDocumentsPath + $@"\MusicService\{album_id}\cover.jpg");
                Directory.Delete(_myDocumentsPath + $@"\MusicService\{album_id}", true);
            }
            catch
            {

            }
        }
        public ICommand ChangeAlbumOrTrackNameCommand { get; }
        private bool CanChangeAlbumOrTrackNameCommandExecute(object p) => !(selectAlbum == null);
        private void OnChangeAlbumOrTrackNameCommandExecuted(object p)
        {
            MainWindowViewModel.selectedVM = new ChangeAlbumViewModel(MainWindowViewModel, selectAlbum);
        }
        public SingerPageViewModel(USERS user,MainWindowViewModel mainWindowViewModel)
        {
            this.user = user;
            Title = user.user_nickname;
            var aLBUMs = context.ALBUMS.Where(a => a.id_user == user.id_user);
            foreach(ALBUMS al in aLBUMs)
            {
                _albumslist.Add(al);
            }
            _MainWindowViewModel = mainWindowViewModel;
            AlbumPageCommand = new LambdaCommand(OnAlbumPageCommandExecuted, CanAlbumPageCommandExecute);
            DeleteAlbumCommand = new LambdaCommand(OnDeleteAlbumCommandExecuted, CanDeleteAlbumCommandExecute);
            ChangeAlbumOrTrackNameCommand = new LambdaCommand(OnChangeAlbumOrTrackNameCommandExecuted, CanChangeAlbumOrTrackNameCommandExecute);
        }
    }
}
