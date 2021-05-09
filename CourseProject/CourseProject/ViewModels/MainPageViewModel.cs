using CourseProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CourseProject.Commands;

namespace CourseProject.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private readonly MusicService db = new MusicService();
        private MainWindowViewModel _MainWindowViewModel;
        private ObservableCollection<ALBUMS> _albumlist = new ObservableCollection<ALBUMS>();
        private int _WitdhAlbums;
        public int WidthAlbums
        {
            get => _WitdhAlbums;
            set => Set(ref _WitdhAlbums, value);
        }
        private USERS _user;
        public USERS user
        {
            get => _user;
            set => Set(ref _user, value);
        }
        private ALBUMS _selectedAlbum;
        public ALBUMS selectedAlbum
        {
            get => _selectedAlbum;
            set => Set(ref _selectedAlbum, value);
        }
        public ObservableCollection<ALBUMS> albumlist
        {
            get => _albumlist;
            set => Set(ref _albumlist, value);
        }
        public ICommand ToAlbumPageCommand { get; }
        private bool CanToAlbumPageCommandExecute(object p) => !(selectedAlbum == null);
        private void OnToAlbumPageCommandExecuted(object p)
        {
            _MainWindowViewModel.selectedVM = new AlbumPageViewModel(_MainWindowViewModel,selectedAlbum);
        }
        public MainPageViewModel(USERS CurrentUser, MainWindowViewModel mainWindowViewModel)
        {
            _MainWindowViewModel = mainWindowViewModel;
            user = CurrentUser;
            var aLBUMs = db.ALBUMS;
            foreach (ALBUMS al in aLBUMs)
            {
                albumlist.Add(al);
            }
            WidthAlbums = 250 * albumlist.Count;
            ToAlbumPageCommand = new LambdaCommand(OnToAlbumPageCommandExecuted, CanToAlbumPageCommandExecute);
        }
    }
}
