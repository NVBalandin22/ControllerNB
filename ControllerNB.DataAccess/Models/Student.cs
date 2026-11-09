using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ControllerNB.Models;
[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(FirstName), nameof(LastName))]
public class Student
{
    [Key]
    public int Id { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    [Required]
    public string FirstName { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    [Required]
    public string LastName { get; set; }
    public int Age { get; set; }
    public int Grade { get; set; }
    public string Password { get; set; }
    
    [Column(TypeName = "nvarchar(100)")]
    public string UserName { get; set; }
    
    [ForeignKey(nameof(Teacher))]
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
}