using System.ComponentModel.DataAnnotations;

namespace BindingApp.Models;

public class Student
{
    [Required(ErrorMessage = "Ism majburiy.")]
    [StringLength(50, ErrorMessage = "Ism 50 ta belgidan oshmasin.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 120, ErrorMessage = "Yosh 1 va 120 oralig'ida bo'lishi kerak.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Yo'nalish majburiy.")]
    [StringLength(100, ErrorMessage = "Yo'nalish 100 ta belgidan oshmasin.")]
    public string Major { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email formati noto'g'ri.")]
    public string? Email { get; set; }
}
