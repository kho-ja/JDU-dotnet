using System.ComponentModel.DataAnnotations;

namespace EfApp.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [StringLength(60)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 120)]
    public int Age { get; set; }

    [StringLength(100)]
    public string Major { get; set; } = string.Empty;
}
