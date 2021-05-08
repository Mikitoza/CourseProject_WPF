using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CourseProject.Commands;
using CourseProject.DataModels;
using CourseProject.Views;
namespace CourseProject.ViewModels
{
    class RegisterPageViewModel : ViewModelBase
    {
        private MusicService context = new MusicService();
        private bool _IsSinger = false;
        public bool IsSinger
        {
            get => _IsSinger;
            set => Set(ref _IsSinger, value);
        }
        private string _Login;
        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }
        private bool _DialogVisible;
        public bool DialogVisible
        {
            get => _DialogVisible;
            set => Set(ref _DialogVisible, value);
        }
        private string _DialogText;
        public string DialogText
        {
            get => _DialogText;
            set => Set(ref _DialogText, value);
        }
        private string _Password;
        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        #region Команды

        #region RegisterUserCommand

        public ICommand RegisterUserCommand { get; }
        private void OnRegisterUserCommandExecuted(object p)
        {
            if (context.USERS.FirstOrDefault(us => us.user_login == Login) != null)
            {
                DialogVisible = true;
                DialogText = "Такой пользователь уже существует";
            }
            else
            {
                this.Password = (p as PasswordBox).Password;
                string password = USERS.getHash(Password);
                USERS user = new USERS(Login, password, IsSinger);
                context.USERS.Add(user);
                context.SaveChanges();
                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(user);
                MainWindow mainWindow = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };
                DialogVisible = true;
                DialogText = "Регистрация прошла успешно";
                mainWindow.Show();
                Application.Current.MainWindow.Close();
            }
        }
        private bool CanRegisterUserCommandExecute(object p) => Login?.Length > 3 && (p as PasswordBox).Password?.Length >= 8;

        public ICommand CloseDialogCommand { get; }
        private bool CanCloseDialogCommandExecute(object p) => true;
        private void OnCloseDialogCommandExecuted(object p) => DialogVisible = false;
        #endregion

        #endregion

        public RegisterPageViewModel()
        {
            RegisterUserCommand = new LambdaCommand(OnRegisterUserCommandExecuted, CanRegisterUserCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted, CanCloseDialogCommandExecute);
        }
    }
}
