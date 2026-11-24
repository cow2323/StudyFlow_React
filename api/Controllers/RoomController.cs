using Microsoft.AspNetCore.Mvc;
using StudyFlow.DAL;
using StudyFlow.DTOs;
using StudyFlow.Models;

namespace StudyFlow.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _repository;

    public RoomController(IRoomRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetAll()
    {
        var rooms = await _repository.GetAll();

        return Ok(rooms.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomDto>> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Rom-ID må være større enn 0.");
        }

        var room = await _repository.GetById(id);

        if (room == null)
        {
            return NotFound("Rommet finnes ikke.");
        }

        return Ok(ToDto(room));
    }

    private static RoomDto ToDto(Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Location = room.Location,
            Capacity = room.Capacity,
            Description = room.Description
        };
    }
}