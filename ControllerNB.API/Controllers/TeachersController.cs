using ControllerNB.DTOs.TeacherDTOs;
using ControllerNB.Filters;
using ControllerNB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControllerNB.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class TeachersController:ControllerBase
{
    private readonly ITeacherService _service;
    public TeachersController(ITeacherService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult GetAll([FromBody] TeacherFilterModel filters)
    {
        var result = _service.GetAll(filters);
        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetOne([FromQuery] int id)
    {
        var result = _service.GetOne(id);
        if(result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create([FromBody] TeacherEditDTO teacher)
    {
        var result = _service.Create(teacher);
        if (result == null) return BadRequest();
        return Created();
    }

    [HttpPut]
    public IActionResult Update([FromBody] TeacherEditDTO teacher)
    {
        var result = _service.Update(teacher);
        if (!result) return BadRequest();
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromQuery] int id)
    {
        var result = _service.Delete(id);
        if (!result) return NotFound();
        return Ok();
    }
}