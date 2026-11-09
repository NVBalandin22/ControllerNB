namespace ControllerNB.Models;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public int Age { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    
    public ICollection<Student> Students { get; set; }
}