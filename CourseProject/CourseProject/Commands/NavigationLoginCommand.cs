using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.Commands.Base;
using CourseProject.Services.Stores;
using CourseProject.ViewModels;

namespace CourseProject.Commands
{
    class NavigationLoginCommand : Command
    {
        private readonly NavigationStore _navigationStore;

        public NavigationLoginCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object param)
        {
            _navigationStore.CurrentViewModel = new AuthPageViewModel();
        }

        public override bool CanExecute(object param) => true;
    }
}
