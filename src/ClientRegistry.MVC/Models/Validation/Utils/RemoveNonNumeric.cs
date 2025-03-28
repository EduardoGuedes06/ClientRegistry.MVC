using System.Text.RegularExpressions;

namespace ClientRegistry.MVC.Models.Validation.Utils
{
    public static class ValidationUtils
    {
        public static Task<string> RemoveNonNumericAsync(string value)
        {
            return Task.FromResult(Regex.Replace(value, "[^0-9]", ""));
        }

    }
}
