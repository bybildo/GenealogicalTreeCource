using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GenealogicalTreeCource.ViewModel
{
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(false, "Ім'я не може бути порожнім.");
            if (input.Length > 20)
                return new ValidationResult(false, "Ім'я завелике.");
            string pattern = @"^[a-zA-Zа-яА-Я]+$"; // Змініть на свій
            if (!Regex.IsMatch(input, pattern))
                return new ValidationResult(false, "Ім'я має містити лише букви.");
            return ValidationResult.ValidResult;
        }
    }

}
