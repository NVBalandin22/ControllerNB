namespace ControllerNB.Filters;

public class StudentFilterModel : BaseFilterModel
{
    public int? Age { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Grade { get; set; }
}