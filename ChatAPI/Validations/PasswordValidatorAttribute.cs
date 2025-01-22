using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ChatAPI.Validations
{
    public class PasswordValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
          
            var password = value as string;
            if (string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult("La contraseña es requerida"); 
            }

            if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$"))
            {
                return new ValidationResult("La contraseña debe tener al menos 8 caracteres, incluir una letra mayúscula, una letra minúscula, un número y un carácter especial.");
            }

            return ValidationResult.Success;


        }
    }
}
