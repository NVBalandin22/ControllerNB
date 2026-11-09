using ControllerNB.DTOs.TeacherDTOs;
using ControllerNB.Filters;
namespace ControllerNB.Services.Interfaces;

public interface ITeacherService
{
    public List<TeacherResponseDTO> GetAll(TeacherFilterModel filters);
    public TeacherResponseDTO? GetOne(int id);
    public int? Create(TeacherEditDTO teacher);
    public bool Update(TeacherEditDTO teacher);
    public bool Delete(int id);
}