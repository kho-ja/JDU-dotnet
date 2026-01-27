namespace RazorApp.Models;

public class RazorDemoViewModel
{
    public string Title { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public int VisitsToday { get; set; }
    public bool IsLoggedIn { get; set; }
    public List<Student> Students { get; set; } = new();
    public StudentForm Form { get; set; } = new();
}
