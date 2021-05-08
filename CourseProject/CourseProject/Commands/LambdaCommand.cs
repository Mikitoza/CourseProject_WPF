using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.Commands.Base;
namespace CourseProject.Commands
{
    class LambdaCommand : Command
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExecute;

        public LambdaCommand(Action<object> Execute, Func<object,bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute; 
        }

        public override bool CanExecute(object param) => _CanExecute?.Invoke(param) ?? true;
        public override void Execute(object param) => _Execute(param);
    }
}
