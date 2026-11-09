using ControllerNB.DTOs.StudentDTOs;
using ControllerNB.Filters;

namespace ControllerNB.Services.Interfaces;

public interface IStudentService
{
    public List<StudentResponseDTO> GetAll(StudentFilterModel filters);
    public StudentResponseDTO? GetOne(int id);
    public int? Create(StudentEditDTO student);
    public bool Update(StudentEditDTO student);
    public bool Delete(int id);
    public bool UpdateGrade(StudentUpdateGradeDTO student);
}