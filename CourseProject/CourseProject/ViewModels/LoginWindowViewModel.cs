using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseProject.Commands;
using CourseProject.Views;
using System.Windows;
using CourseProject.DataModels;

namespace CourseProject.ViewModels
{
    class LoginWindowViewModel : ViewModelBase
    {
        private bool _IsRegister = false;
        public bool IsRegister
        {
            get => _IsRegister;
            set => Set(ref _IsRegister, value);
        }
        private bool _IsLogin = true;
        public bool IsLogin
        {
            get => _IsLogin;
            set => Set(ref _IsLogin, value);
        }
        private ViewModelBase _selectedVM = new AuthPageViewModel();
        public ViewModelBase selectedVM
        {
            get => _selectedVM;
            set => Set(ref _selectedVM, value);
        }

        public ICommand SwitchViewCommand { get; }
        private bool CanSwitchViewCommandExecute(object p) => true;
        private void OnSwitchViewCommandExecuted(object p)
        {
            switch (p.ToString())
            {
                case "RegisterPage":
                    selectedVM = new RegisterPageViewModel();
                    IsRegister = true;
                    IsLogin = false;
                    break;
                case "AuthPage":
                    selectedVM = new AuthPageViewModel();
                    IsRegister = false;
                    IsLogin = true;
                    break;
            }
        }
        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object p) => true;
        public LoginWindowViewModel()
        {
            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            SwitchViewCommand = new LambdaCommand(OnSwitchViewCommandExecuted, CanSwitchViewCommandExecute);
            LoginWindowViewModel.DataBaseAsync();

        }
        static void DataBase()
        {
            MusicService musicService = new MusicService();
            musicService.USERS.FirstOrDefault(u => u.id_user == 1);
        }

        static async void DataBaseAsync()
        {
            await Task.Run(() => DataBase());
        }
    }
}
