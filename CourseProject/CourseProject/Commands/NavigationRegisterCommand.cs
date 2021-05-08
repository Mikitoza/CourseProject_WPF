using CourseProject.Commands.Base;
using CourseProject.Services.Stores;
using CourseProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Commands
{
    class NavigationRegisterCommand : Command
    {
        private readonly NavigationStore _navigationStore;

        public NavigationRegisterCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object param)
        {
            _navigationStore.CurrentViewModel = new RegisterPageViewModel();
        }

        public override bool CanExecute(object param) => true;
    }
}
