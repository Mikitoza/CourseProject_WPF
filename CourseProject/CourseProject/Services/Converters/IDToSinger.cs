using CourseProject.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CourseProject.Services.Converters
{
    class IDToSinger : IValueConverter
    {
        MusicService db = new MusicService();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $@"\MusicService\{(int)value}\cover.jpg"))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $@"\MusicService\{(int)value}\cover.jpg";
            }
            else
            {
                return "/Resources/noPhoto.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
