using ControllerNB.DTOs.StudentDTOs;
using ControllerNB.Filters;
using ControllerNB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControllerNB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult GetAll([FromBody] StudentFilterModel filters)
    {
        var result = _service.GetAll(filters);
        
        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetOne([FromQuery] int id)
    {
        var result = _service.GetOne(id);

        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create([FromBody] StudentEditDTO createStudent)
    {
        var result = _service.Create(createStudent);

        if (result == null)
        {
            return BadRequest();
        }
        
        return Created();
    }

    [HttpPut]
    public IActionResult Update([FromBody] StudentEditDTO updateStudent)
    {
        var result = _service.Update(updateStudent);

        if (!result)
        {
            return BadRequest();
        }
        
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromQuery] int id)
    {
        var result = _service.Delete(id);

        if (!result)
        {
            return NotFound();
        }
        
        return Ok();
    }

    [HttpPatch]
    public IActionResult UpdateGrade([FromBody] StudentUpdateGradeDTO updateStudent)
    {
        var result = _service.UpdateGrade(updateStudent);

        if (!result)
        {
            return BadRequest();
        }
        
        return Ok();
    }
}