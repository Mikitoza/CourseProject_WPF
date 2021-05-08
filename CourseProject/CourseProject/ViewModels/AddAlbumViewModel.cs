using CourseProject.Commands;
using CourseProject.DataModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CourseProject.ViewModels
{
    class AddAlbumViewModel : ViewModelBase
    {
        private readonly MusicService context = new MusicService();
        private readonly string _myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private readonly USERS user;
        private readonly BitmapImage _noPhoto = new BitmapImage(new Uri(@"pack://application:,,,/Resources/noimage.png"));
        private ObservableCollection<GENRES> _genres = new ObservableCollection<GENRES>(new MusicService().GENRES);
        public ObservableCollection<GENRES> genres
        {
            get => _genres;
            set => Set(ref _genres, value);
        }
        private GENRES _selectedGenre;
        public GENRES selectedGenre
        {
            get => _selectedGenre;
            set => Set(ref _selectedGenre, value);
        }
        private string _GenreName;
        public string GenreName
        {
            get => _GenreName;
            set => Set(ref _GenreName, value);
        }
        private string _AlbumName;
        public string AlbumName
        {
            get => _AlbumName;
            set => Set(ref _AlbumName, value);
        }
        private bool _dialog = false;
        public bool dialog
        {
            get => _dialog;
            set => Set(ref _dialog, value);
        }
        private string _dialogText;
        public string dialogText
        {
            get => _dialogText;
            set => Set(ref _dialogText, value);
        }
        private BitmapImage _albumPicture;
        public BitmapImage albumPicture
        {
            get => _albumPicture;
            set => Set(ref _albumPicture, value);
        }
        private string _imgPath;
        private Visibility _fileCheck = Visibility.Collapsed;
        public Visibility fileCheck
        {
            get => _fileCheck;
            set => Set(ref _fileCheck, value);
        }
        private void RestoreForm()
        {
            genres = new ObservableCollection<GENRES>(context.GENRES);
            AlbumName = null;
            albumPicture = _noPhoto;
            fileCheck = Visibility.Collapsed;
            _imgPath = null;  
        }
        public ICommand GenreAddCommand { get; }
        private bool CanGenreAddCommandExecute(object p) => GenreName != null;
        private void OnGenreAddCommandExecuted(object p)
        {
            if (context.GENRES.FirstOrDefault(g => g.genre == GenreName) == null)
            {
                context.GENRES.Add(new GENRES { genre = GenreName });
                context.SaveChanges();
                GenreName = null;
                genres = new ObservableCollection<GENRES>(context.GENRES);
            }
            else
            {
                dialogText = "Такой жанр уже есть.";
                dialog = true;
            }
        }
        public ICommand SelectImagePathCommand { get; }
        private bool CanSelectImagePathCommandExecute(object p) => true;
        private void OnSelectImagePathCommandExecuted(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Фотографии|*.jpg;*.png;*.jpeg;";
            if (openFileDialog.ShowDialog() == true)
            {
                albumPicture = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                _imgPath = openFileDialog.FileName;
            }
        }
        public ICommand CloseDialogCommand { get; }
        private bool CanCloseDialogCommandExecute(object p) => true;
        private void OnCloseDialogCommandExecuted(object p) => dialog = false;
        public ICommand AlbumUploadCommand { get; }
        private bool CanAlbumUploadCommandExecute(object p) => AlbumName != null &&  _imgPath != null;
        private void OnAlbumUploadCommandExecuted(object p)
        {
            ALBUMS album = new ALBUMS(user.id_user,selectedGenre.genre_id,AlbumName);
            context.ALBUMS.Add(album);
            context.SaveChanges();
            ALBUMS albumNeedId = context.ALBUMS.First(a => a.albums_name == album.albums_name);
            Directory.CreateDirectory(_myDocumentsPath + @"\MusicService");
            Directory.CreateDirectory(_myDocumentsPath + $@"\MusicService\");
            Directory.CreateDirectory(_myDocumentsPath + $@"\MusicService\{albumNeedId.album_id}");
            File.Copy(_imgPath, _myDocumentsPath + $@"\MusicService\{album.album_id}\cover.jpg", true);
            RestoreForm();
        }
        public AddAlbumViewModel(USERS user)
        {
            this.user = user;
            this.albumPicture = _noPhoto;
            GenreAddCommand = new LambdaCommand(OnGenreAddCommandExecuted, CanGenreAddCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted, CanCloseDialogCommandExecute);
            SelectImagePathCommand = new LambdaCommand(OnSelectImagePathCommandExecuted, CanSelectImagePathCommandExecute);
            AlbumUploadCommand = new LambdaCommand(OnAlbumUploadCommandExecuted, CanAlbumUploadCommandExecute);
            SelectImagePathCommand = new LambdaCommand(OnSelectImagePathCommandExecuted, CanSelectImagePathCommandExecute);
        }
    }
}
