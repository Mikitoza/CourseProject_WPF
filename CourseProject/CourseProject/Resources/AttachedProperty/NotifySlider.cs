using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CourseProject.Resources.AttachedProperty
{
    public class NotifyingSlider : Slider
    {

        public static readonly DependencyProperty ValueChangedFromUIProperty =
            DependencyProperty.Register("ValueChangedFromUI", typeof(ICommand), typeof(NotifyingSlider));

        public ICommand ValueChangedFromUI
        {
            get
            {
                return (ICommand)GetValue(ValueChangedFromUIProperty);
            }
            set
            {
                SetValue(ValueChangedFromUIProperty, value);
            }
        }

        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            base.OnThumbDragCompleted(e);
            ValueChangedFromUI?.Execute(null);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ValueChangedFromUI?.Execute(null);
        }
    }
}
