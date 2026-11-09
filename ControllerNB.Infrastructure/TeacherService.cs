using ControllerNB.Data;
using ControllerNB.DTOs.TeacherDTOs;
using ControllerNB.Filters;
using ControllerNB.Models;
using ControllerNB.Services.Interfaces;

namespace ControllerNB.Services;

public class TeacherService : ITeacherService
{
    private readonly ApplicationDbContext _context;
    
    public TeacherService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<TeacherResponseDTO> GetAll(TeacherFilterModel filters)
    {
        IQueryable<Teacher> baseQuery = _context.Teachers;

        if (!string.IsNullOrEmpty(filters.Subject))
        {
            baseQuery = baseQuery.Where(t => t.Subject.Contains(filters.Subject));
        }
        if (!string.IsNullOrEmpty(filters.Name))
        {
            baseQuery = baseQuery.Where(t => t.Name.Contains(filters.Name));
        }
        if (filters.Age.HasValue)
        {
            baseQuery = baseQuery.Where(teacher => teacher.Age == filters.Age.Value);
        }
        
        baseQuery = baseQuery.Skip((filters.Page-1) * filters.PageSize).Take(filters.PageSize);
        
        IQueryable<TeacherResponseDTO> query = baseQuery.Select(x=> 
            new TeacherResponseDTO()
            {
                Age = x.Age,
                Id = x.Id,
                Name = x.Name,
                Subject = x.Subject,
                
            });
        return query.ToList();
    }

    public TeacherResponseDTO? GetOne(int id)
    {
        TeacherResponseDTO? teacher = _context.Teachers
            .Where(teacher => teacher.Id == id)
            .Select(teacher =>
                new TeacherResponseDTO()
                {
                    Id = teacher.Id,
                    Age = teacher.Age,
                    Name = teacher.Name,
                    Subject = teacher.Subject
                }
            ).FirstOrDefault();
        return teacher;
    }

    public int? Create(TeacherEditDTO teacher)
    {
        Teacher entity = new Teacher();
        entity.Age = teacher.Age;
        entity.Name = teacher.Name;
        entity.Subject = teacher.Subject;
        entity.UserName = teacher.UserName;
        entity.Password = "TempPassword";

        try
        {
            _context.Teachers.Add(entity);
            _context.SaveChanges();
        }
        catch
        {
            return null;
        }
        
        return entity.Id;
    }

    public bool Update(TeacherEditDTO teacher)
    {
        Teacher? entity = _context.Teachers.Where(t => t.Id==teacher.Id).FirstOrDefault();
        if (entity == null) return false;
        entity.Age = teacher.Age;
        entity.Name = teacher.Name;
        entity.Subject = teacher.Subject;
        entity.UserName = teacher.UserName;
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        Teacher? entity = _context.Teachers.Where(t=>t.Id == id).FirstOrDefault();
        if (entity == null) return false;
        _context.Teachers.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}