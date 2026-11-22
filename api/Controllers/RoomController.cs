using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyFlow.DAL;
using StudyFlow.DTOs;
using StudyFlow.Models;

namespace StudyFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomAPIController : ControllerBase
{
    private readonly StudyFlowDbContext _context;
    private readonly ILogger<RoomAPIController> _logger;

    public RoomAPIController(StudyFlowDbContext context, ILogger<RoomAPIController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET api/RoomAPI/roomlist
    [HttpGet("roomlist")]
    public async Task<IActionResult> RoomList()
    {
        var roomDtos = await _context.Rooms
            .OrderBy(r => r.Name)
            .Select(room => new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Location = room.Location,
                Capacity = room.Capacity,
                Description = room.Description
            })
            .ToListAsync();

        return Ok(roomDtos);
    }

    // GET api/RoomAPI/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be 1 or greater");
        }

        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            _logger.LogWarning("[RoomAPIController] Room with id {RoomId} not found", id);
            return NotFound("Room not found");
        }

        return Ok(ToDto(room));
    }

    // POST api/RoomAPI/create
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom([FromBody] RoomDto roomDto)
    {
        // [ApiController] already returns 400 automatically if the DTO validation fails.

        string name = roomDto.Name.Trim();

        bool nameTaken = await _context.Rooms.AnyAsync(r => r.Name == name);
        if (nameTaken)
        {
            return BadRequest("A room with this name already exists");
        }

        var newRoom = new Room
        {
            // Id is ignored on create; the database generates it.
            Name = name,
            Location = roomDto.Location.Trim(),
            Capacity = roomDto.Capacity,
            Description = string.IsNullOrWhiteSpace(roomDto.Description)
                ? null
                : roomDto.Description.Trim()
        };

        try
        {
            _context.Rooms.Add(newRoom);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "[RoomAPIController] Creating room {RoomName} failed", name);
            return StatusCode(500, "Internal Server Error");
        }

        return CreatedAtAction(nameof(Details), new { id = newRoom.Id }, ToDto(newRoom));
    }

    private static RoomDto ToDto(Room room) => new RoomDto
    {
        Id = room.Id,
        Name = room.Name,
        Location = room.Location,
        Capacity = room.Capacity,
        Description = room.Description
    };
}
