using ControllerNB.Data;
using ControllerNB.DTOs.StudentDTOs;
using ControllerNB.Filters;
using ControllerNB.Models;
using ControllerNB.Services.Interfaces;

namespace ControllerNB.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    
    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<StudentResponseDTO> GetAll(StudentFilterModel filters)
    {
        IQueryable<Student> baseQuery = _context.Students;

        if (filters.Grade.HasValue)
        {
            baseQuery = baseQuery.Where(student => student.Grade == filters.Grade.Value);
        }
        if (filters.Age.HasValue)
        {
            baseQuery = baseQuery.Where(student => student.Age == filters.Age.Value);
        }
        if (!string.IsNullOrEmpty(filters.FirstName))
        {
            // WHERE FIRSTNAME LIKE '%asdadasd%'
            baseQuery = baseQuery.Where(student => student.FirstName.Contains(filters.FirstName));
        }
        if (!string.IsNullOrEmpty(filters.LastName))
        {
            baseQuery = baseQuery.Where(student => student.LastName.Contains(filters.LastName));
        }
        
        baseQuery = baseQuery.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize);

        IQueryable<StudentResponseDTO> query = baseQuery.Select(x =>
            new StudentResponseDTO()
            {
                Id = x.Id,
                Names = x.FirstName + " " + x.LastName,
                Age = x.Age
            });
        
        return query.ToList();
    }

    public StudentResponseDTO? GetOne(int id)
    {
        StudentResponseDTO? student = _context.Students
            .Where(student => student.Id == id)
            .Select(student =>
                new StudentResponseDTO()
                {
                    Id = student.Id,
                    Age = student.Age,
                    Names = student.FirstName + " " + student.LastName
                }
            ).FirstOrDefault();

        return student;
    }

    public int? Create(StudentEditDTO createStudent)
    {
        Student entity = new Student();
        
        entity.FirstName = createStudent.FirstName;
        entity.LastName = createStudent.LastName;
        entity.Age = createStudent.Age;
        entity.UserName = createStudent.UserName;
        entity.Grade = 0;
        entity.TeacherId = createStudent.TeacherId;
        entity.Password = "TempPassword";

        try
        {
            _context.Students.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            return null;
        }
        
        return entity.Id;
    }

    public bool Update(StudentEditDTO updateStudent)
    {
        Student? entity = _context.Students
            .Where(student => student.Id == updateStudent.Id)
            .FirstOrDefault();

        if (entity == null)
        {
            return false;
        }
        
        entity.FirstName = updateStudent.FirstName;
        entity.LastName = updateStudent.LastName;
        entity.TeacherId = updateStudent.TeacherId;
        entity.Age = updateStudent.Age;
        entity.UserName = updateStudent.UserName;
        
        
        try
        {
            _context.Students.Update(entity);
            _context.SaveChanges();
            return true;
            
        }
        catch (Exception e)
        {
            return false;
        }
        
    }

    public bool Delete(int id)
    {
        Student? entity = _context.Students
            .Where(student => student.Id == id)
            .FirstOrDefault();

        if (entity == null)
        {
            return false;
        }
        
        _context.Students.Remove(entity);
        _context.SaveChanges();
        
        return true;
    }

    public bool UpdateGrade(StudentUpdateGradeDTO updateStudent)
    {
        Student entity = _context.Students
            .Where(student => student.Id == updateStudent.StudentId)
            .First();

        entity.Grade = updateStudent.NewGrade;
        
        bool result = _context.SaveChanges() == 1;
        
        return result;
    }
}