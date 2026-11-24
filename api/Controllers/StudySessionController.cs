using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyFlow.Models;
using StudyFlow.DTOs;
using StudyFlow.DAL;
using System.Security.Claims;

namespace StudyFlow.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudySessionAPIController : ControllerBase
{
    private readonly IStudySessionRepository _sessionRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<StudySessionAPIController> _logger;


    public StudySessionAPIController(
        IStudySessionRepository sessionRepository,
        IBookingRepository bookingRepository,
        ILogger<StudySessionAPIController> logger)
    {
        _sessionRepository = sessionRepository;
        _bookingRepository = bookingRepository;
        _logger = logger;
    }


    [HttpGet("sessionlist")]
    [Authorize]
    public async Task<IActionResult> SessionList()
    {
        //Return List of all sessions

        var sessions = await _sessionRepository.GetAll();

        if (sessions == null)
        {
            _logger.LogError("[StudySessionAPIController] Session list cannot be found while executing GetAll()");
            return NotFound("Session list not found");
        }
        IEnumerable<StudySession> sessionList = sessions.OrderBy(s => s.StartTime); 
        var sessionDtos = sessionList.Select(ToDto);

        return Ok(sessionDtos);
    }



    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var session = await _sessionRepository.GetById(id);

        if (session == null)
        {
            return NotFound("Study session not found");
        }

        return Ok(ToDto(session));
    }


    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] StudySessionDto sessionDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Must be logged in");
        }

        DateTime start = sessionDto.StartTime.ToUniversalTime();
        DateTime end = sessionDto.EndTime.ToUniversalTime();

        string? error = await ValidateSessionAsync(sessionDto.RoomId, start, end);
        if (error != null)
        {
            return BadRequest(error);
        }

        // StudySession inherits from Booking, so the session IS the room booking.
        // UserId on the session is the host.
        var newSession = new StudySession
        {
            UserId = userId,
            RoomId = sessionDto.RoomId,
            StartTime = start,
            EndTime = end,
            IsStudySession = true,
            Course = sessionDto.Course,
            Topic = sessionDto.Topic,
            Description = sessionDto.Description,
            IsPublic = sessionDto.IsPublic
        };

        // The creator joins the session as host.
        newSession.Participants.Add(new StudySessionParticipant
        {
            Session = newSession,
            UserId = userId,
            IsHost = true
        });

        bool returnOk = await _sessionRepository.CreateAsync(newSession);
        if (returnOk)
        {
            return CreatedAtAction(nameof(GetById), new { id = newSession.Id }, ToDto(newSession));
        }

        _logger.LogWarning("[StudySessionAPIController] Creating study session failed for room {RoomId}", sessionDto.RoomId);
        return StatusCode(500, "Internal Server Error");
    }


    [HttpPut("update/{id:int}")]
    [Authorize]
    public async Task<IActionResult> Edit(int id, [FromBody] StudySessionDto sessionDto)
    {
        var session = await _sessionRepository.GetById(id);
        if (session == null)
        {
            return NotFound("Study session not found");
        }

        // Only the host can edit the session.
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (session.UserId != userId) return Forbid();

        DateTime start = sessionDto.StartTime.ToUniversalTime();
        DateTime end = sessionDto.EndTime.ToUniversalTime();

        // Exclude this session from the overlap check so it doesn't collide with itself.
        string? error = await ValidateSessionAsync(sessionDto.RoomId, start, end, excludeBookingId: id);
        if (error != null)
        {
            return BadRequest(error);
        }

        // Moving to a smaller room must still fit everyone who has joined.
        int? capacity = await _sessionRepository.GetRoomCapacity(sessionDto.RoomId);
        if (capacity < session.Participants.Count)
        {
            return BadRequest("The new room is too small for the current participants.");
        }

        session.RoomId = sessionDto.RoomId;
        session.StartTime = start;
        session.EndTime = end;
        session.Course = sessionDto.Course;
        session.Topic = sessionDto.Topic;
        session.Description = sessionDto.Description;
        session.IsPublic = sessionDto.IsPublic;

        bool returnOk = await _sessionRepository.UpdateAsync(session);
        if (returnOk)
        {
            return Ok(ToDto(session));
        }

        _logger.LogWarning("[StudySessionAPIController] Updating study session {SessionId} failed", id);
        return StatusCode(500, "Internal Server Error");
    }


    [HttpDelete("delete/{id:int}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var session = await _sessionRepository.GetById(id);
        if (session == null)
        {
            return NotFound("Study session not found");
        }

        // Only the host can delete the session.
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (session.UserId != userId) return Forbid();

        bool returnOk = await _sessionRepository.DeleteAsync(id);
        if (returnOk)
        {
            return NoContent();
        }

        _logger.LogWarning("[StudySessionAPIController] Deleting study session {SessionId} failed", id);
        return StatusCode(500, "Internal Server Error");
    }


    [HttpPut("join/{id:int}")]
    [Authorize]
    public async Task<IActionResult> Join(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Must be logged in");
        }

        var session = await _sessionRepository.GetById(id);
        if (session == null)
        {
            return NotFound("Study session not found");
        }

        if (!session.IsPublic)
        {
            return BadRequest("Session is private");
        }

        if (session.Participants.Any(p => p.UserId == userId))
        {
            return BadRequest("You have already joined this session");
        }

        int? capacity = await _sessionRepository.GetRoomCapacity(session.RoomId);
        if (capacity == null || session.Participants.Count >= capacity)
        {
            return BadRequest("Session is at full capacity");
        }

        var newParticipant = new StudySessionParticipant
        {
            Session = session,
            UserId = userId,
            IsHost = false
        };

        bool returnOk = await _sessionRepository.AddParticipantAsync(newParticipant);
        if (returnOk)
        {
            return Ok(ToDto(session));
        }

        _logger.LogWarning("[StudySessionAPIController] Joining study session {SessionId} failed", id);
        return StatusCode(500, "Internal Server Error");
    }


    // Shared rules for Create and Edit. Returns an error message, or null if valid.
    private async Task<string?> ValidateSessionAsync(int roomId, DateTime start, DateTime end, int? excludeBookingId = null)
    {
        if (start < DateTime.UtcNow)
        {
            return "Start time must be in the future";
        }

        if (end <= start)
        {
            return "End time must be after start time";
        }

        if (await _sessionRepository.GetRoomCapacity(roomId) == null)
        {
            return "Room does not exist";
        }

        // Study sessions are stored in the Bookings table, so this checks against
        // both normal bookings and other study sessions.
        bool overlap = await _bookingRepository.HasOverlapAsync(roomId, start, end, excludeBookingId);
        if (overlap)
        {
            return "Room is already booked in this time slot";
        }

        return null;
    }


    private static StudySessionDto ToDto(StudySession session) => new StudySessionDto
    {
        //Helper method to make DTO objects
        Id = session.Id,
        RoomId = session.RoomId,
        Course = session.Course,
        Topic = session.Topic,
        Description = session.Description,
        StartTime = session.StartTime,
        EndTime = session.EndTime,
        IsPublic = session.IsPublic,
        ParticipantCount = session.Participants.Count
    };
}
