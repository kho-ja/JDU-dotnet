using System.ComponentModel.DataAnnotations;

namespace ValidationApp.Models;

public class StudentRegistration : IValidatableObject
{
    [Required(ErrorMessage = "Ism majburiy.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ism 2-50 belgidan iborat bo'lishi kerak.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Familia majburiy.")]
    [StringLength(50, ErrorMessage = "Familia 50 belgidan oshmasin.")]
    public string Surname { get; set; } = string.Empty;

    [Range(15, 80, ErrorMessage = "Yosh 15 va 80 oralig'ida bo'lishi kerak.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Yo'nalish majburiy.")]
    public string Major { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email formati noto'g'ri.")]
    public string? Email { get; set; }

    [StudentId(ErrorMessage = "Student ID formatini tekshiring. Masalan: ST-1234")]
    public string? StudentId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Age < 18 && string.Equals(Major, "Software Engineering", StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                "Software Engineering yo'nalishi uchun yosh 18 dan kichik bo'lmasligi kerak.",
                new[] { nameof(Age), nameof(Major) });
        }
    }
}
