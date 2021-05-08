using CourseProject.Commands;
using CourseProject.DataModels;
using CourseProject.DataModels.ENUM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProject.ViewModels
{
    class AdminPageViewModel : ViewModelBase
    {
        private readonly MusicService context = new MusicService();
        private USERS _selectUser;
        public USERS selectUser
        {
            get => _selectUser;
            set => Set(ref _selectUser, value);
        }
        private ObservableCollection<USERS> _usersList = new ObservableCollection<USERS>();
        public ObservableCollection<USERS> usersList
        {
            get => _usersList;
            set => Set(ref _usersList, value);
        }
        private void RestoreForm()
        {
            usersList = new ObservableCollection<USERS>();
            var users = context.USERS;
            foreach (USERS useR in users)
            {
                usersList.Add(useR);
            }
        }
        public ICommand MakeUserSingerCommand { get; }
        private bool CanMakeUserSingerCommandExecute(object p) => selectUser?.user_role == (int)UserRole.User;
        private void OnMakeUserSingerCommandExecuted(object p)
        {
            var ChangeUser = context.USERS.Find(selectUser.id_user);
            if (ChangeUser != null)
            {
                ChangeUser.user_role = (int)UserRole.Singer;
                context.SaveChanges();
            }
        }
        public ICommand MakeSingerUserCommand { get; }
        private bool CanMakeSingerUserCommandExecute(object p) => selectUser?.user_role == (int)UserRole.Singer;
        private void OnMakeSingerUserCommandExecuted(object p)
        {
            var ChangeUser = context.USERS.Find(selectUser.id_user);
            if (ChangeUser != null)
            {
                ChangeUser.user_role = (int)UserRole.User;
                context.SaveChanges();
            }
        }
        public ICommand DeleteCommand { get; }
        private bool CanDeleteCommandExecute(object p) => selectUser != null &&  selectUser.id_user != (int)UserRole.Admin;
        private void OnDeleteCommandExecuted(object p)
        {
            foreach(TRACKS tr in context.TRACKS.Where(tr => tr.id_user == selectUser.id_user))
            {
                context.TRACKS.Remove(tr);
            }
            foreach(ALBUMS al in context.ALBUMS.Where(al => al.id_user == selectUser.id_user))
            {
                context.ALBUMS.Remove(al);
            }
            context.USERS.Remove(selectUser);
            context.SaveChanges();
            RestoreForm();
        }
        public AdminPageViewModel()
        {
            foreach (USERS us in context.USERS)
            {
                usersList.Add(us);
            }
            MakeSingerUserCommand = new LambdaCommand(OnMakeSingerUserCommandExecuted,CanMakeSingerUserCommandExecute);
            MakeUserSingerCommand = new LambdaCommand(OnMakeUserSingerCommandExecuted, CanMakeUserSingerCommandExecute);
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
        }
    }
}
