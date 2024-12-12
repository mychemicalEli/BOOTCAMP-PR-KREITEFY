using System.Text.RegularExpressions;

namespace api.Domain.Validators;

public static class UserValidator
{
    private const string PasswordStrengthPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&_\-])[A-Za-z\d@$!%*?&_\-]{8,}$";
    public static bool IsValidEmail(string email)
    {
        return !string.IsNullOrEmpty(email) && new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);
    }

    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) return false;
        password = password.Trim();
        return Regex.IsMatch(password, PasswordStrengthPattern);
    }
}