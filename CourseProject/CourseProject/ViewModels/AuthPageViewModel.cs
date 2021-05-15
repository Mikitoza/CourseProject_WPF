using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CourseProject.DataModels;
using CourseProject.Commands;
using CourseProject.Views;
using System.Windows.Controls;

namespace CourseProject.ViewModels
{
    class AuthPageViewModel : ViewModelBase
    {
        private MusicService dbcontext = new MusicService();
        private USERS _user;
        public USERS user
        {
            get => _user;
            set => Set(ref _user, value);
        }
        private ViewModelBase _selectedVM;
        public ViewModelBase selectedVM
        {
            get => _selectedVM;
            set => Set(ref _selectedVM, value);
        }
        private bool _DialogVisible;
        public bool DialogVisible
        {
            get => _DialogVisible;
            set => Set(ref _DialogVisible,value);
        }
        private string _DialogText;
        public string DialogText
        {
            get => _DialogText;
            set => Set(ref _DialogText, value);
        }
        private string _Login;
        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }

        private string _Password;

        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        #region Команды

        #region AuthCommand

        public ICommand AuthCommand { get; }

        public void OnAuthCommandExecuted (object p)
        {
            Password = USERS.getHash(((p as PasswordBox).Password));
            try
            {
                if (dbcontext.USERS.FirstOrDefault(u => u.user_login == Login && u.user_password == Password) != null)
                {
                    USERS user = dbcontext.USERS.FirstOrDefault(u => u.user_login == Login);
                    MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(user);
                    MainWindow mainWindow = new MainWindow()
                    {
                        DataContext = mainWindowViewModel
                    };
                    mainWindow.Show();
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    DialogVisible = true;
                    if (App.Language.Name == "ru-Ru")
                    {
                        DialogText = "Такой пользователь не найден";
                    }
                    else
                    {
                        DialogText = "This user is not found";
                    }
                }
            }
            catch(Exception ex)
            {
                DialogVisible = true;
                if(App.Language.Name == "ru-Ru")
                {
                    DialogText = "Нет подключения к интернету";
                }
                else
                {
                    DialogText = "Can not connect to database";
                }
            }
        }

        public bool CanAuthCommandExecute(object p) => Login?.Length > 3 & Login?.Length < 20 && (p as PasswordBox).Password?.Length >= 8;
        #endregion
        public ICommand CloseDialogCommand { get; }
        private bool CanCloseDialogCommandExecute(object p) => true;
        private void OnCloseDialogCommandExecuted(object p) => DialogVisible = false;
        #endregion

        public AuthPageViewModel()
        {
            #region Команды
            AuthCommand = new LambdaCommand(OnAuthCommandExecuted, CanAuthCommandExecute);
            CloseDialogCommand = new LambdaCommand(OnCloseDialogCommandExecuted,CanCloseDialogCommandExecute);
            #endregion
        }

    }
}
