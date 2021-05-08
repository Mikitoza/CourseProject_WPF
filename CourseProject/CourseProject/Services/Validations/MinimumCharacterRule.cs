using System.Globalization;
using System.Windows.Controls;

namespace CourseProject.Services.Validations
{
    class MinimumCharacterRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;
                if (charString.Length < MinimumCharacters)
                    return new ValidationResult(false, $"Minimun = {MinimumCharacters} characters.");
            
            return ValidationResult.ValidResult;
        }
    }
}
