using CourseProject.Commands;
using CourseProject.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProject.ViewModels
{
    class ProfilePageViewModel : ViewModelBase
    {
        private readonly MusicService context = new MusicService();
        private MainWindowViewModel mainWindowViewModel;
        public MainWindowViewModel MainWindowViewModel
        {
            get => mainWindowViewModel;
            set => Set(ref mainWindowViewModel, value);
        }
        private USERS _user;
        public USERS user
        {
            get => _user;
            set => Set(ref _user, value);
        }
        private ObservableCollection<TRACKS> _usersTracksList = new ObservableCollection<TRACKS>();
        public ObservableCollection<TRACKS> usersTracksList
        {
            get => _usersTracksList;
            set => Set(ref _usersTracksList, value);
        }
        private TRACKS _selectTrack;
        public TRACKS selectTrack
        {
            get => _selectTrack;
            set => Set(ref _selectTrack, value);
        }
        private string _nickname;
        public string nickname
        {
            get => _nickname;
            set => Set(ref _nickname, value);
        }
        public ICommand ChangeNickNameCommand { get; }
        private bool CanChangeNickNameCommanddExecute(object p) => nickname?.Length > 3;
        private void OnChangeNickNameCommandExecuted(object p)
        {
            var ChangeUser = context.USERS.Find(user.id_user);
            if (ChangeUser != null)
            {
                ChangeUser.user_nickname = nickname;
                context.SaveChanges();
            }
            nickname = null;
        }
        public ICommand DeleteCommand { get; }
        private bool CanDeleteCommandExecute(object p) => true;
        private void OnDeleteCommandExecuted(object p)
        {
            USERS us = context.USERS.Find(user.id_user);
            foreach(TRACKS tr in context.TRACKS.Where(tr => tr.id_user == user.id_user))
            {
                context.TRACKS.Remove(tr);
            }
            foreach (ALBUMS al in context.ALBUMS.Where(al => al.id_user == user.id_user))
            {
                context.ALBUMS.Remove(al);
            }
            context.USERS.Remove(us);
            context.SaveChanges();
            App.Current.Shutdown();
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
        public ProfilePageViewModel(USERS user,MainWindowViewModel main)
        {
            this.user = user;
            this.MainWindowViewModel = main;
            foreach (TRACKS us in context.USERS.Find(user.id_user).TRACKS1)
            {
                _usersTracksList.Add(us);
            }
            ChangeNickNameCommand = new LambdaCommand(OnChangeNickNameCommandExecuted, CanChangeNickNameCommanddExecute);
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
            AddTrackToUserTracksCommand = new LambdaCommand(OnAAddTrackToUserTracksCommandExecuted, CanAddTrackToUserTracksCommandExecute);
            DeleteTrackToUserTracksCommand = new LambdaCommand(OnDeleteTrackToUserTracksCommandExecuted, CanDeleteTrackToUserTracksCommandExecute);
            LoadCommand = new LambdaCommand(OnLoadCommandExecuted, CanLoadCommandExecute);
        }
    }
}
