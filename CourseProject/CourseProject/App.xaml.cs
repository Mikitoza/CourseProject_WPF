using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
		private static List<CultureInfo> m_Languages = new List<CultureInfo>();

		public static List<CultureInfo> Languages
		{
			get
			{
				return m_Languages;
			}
		}

		public App()
		{
			m_Languages.Clear();
			m_Languages.Add(new CultureInfo("en-US")); //Нейтральная культура для этого проекта
			m_Languages.Add(new CultureInfo("ru-RU"));
		}

		public static CultureInfo Language
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

				//1. Меняем язык приложения:
				System.Threading.Thread.CurrentThread.CurrentUICulture = value;

				//2. Создаём ResourceDictionary для новой культуры
				ResourceDictionary dict = new ResourceDictionary();
				switch (value.Name)
				{
					case "ru-RU":
						dict.Source = new Uri(String.Format("Services/Language/DictionaryRusxaml.xaml", value.Name), UriKind.Relative);
						ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
													  where d.Source != null && d.Source.OriginalString.StartsWith("Services/Language/DictionaryEng.xaml")
													  select d).First();
						int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
						Application.Current.Resources.MergedDictionaries.Remove(oldDict);
						Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
						break;
					default:
						dict.Source = new Uri("Services/Language/DictionaryEng.xaml", UriKind.Relative);
						ResourceDictionary oldDict1 = (from d in Application.Current.Resources.MergedDictionaries
													  where d.Source != null && d.Source.OriginalString.StartsWith("Services/Language/DictionaryRusxaml.xaml")
													  select d).First();
						int ind1 = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict1);
						Application.Current.Resources.MergedDictionaries.Remove(oldDict1);
						Application.Current.Resources.MergedDictionaries.Insert(ind1, dict);
						break;
				}

			}
		}
	}
}
