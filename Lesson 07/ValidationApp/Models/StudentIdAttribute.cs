using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ValidationApp.Models;

public class StudentIdAttribute : ValidationAttribute
{
    private static readonly Regex Pattern = new("^ST-\\d{4}$", RegexOptions.Compiled);

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        var text = value.ToString();
        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        return Pattern.IsMatch(text);
    }
}
