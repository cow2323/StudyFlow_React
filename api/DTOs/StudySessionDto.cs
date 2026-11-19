using System.ComponentModel.DataAnnotations;

namespace StudyFlow.DTOs;

public class StudySessionDto
{
    // Sent to React so it can edit/delete/join a session. Ignored on create.
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Value Must be 1 or greater")]
    public int RoomId { get; set; }

    [Required]
    [StringLength(100)]
    public string Course { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Topic { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public bool IsPublic { get; set; } = true;

    // Read-only info for the client. Ignored on create/edit.
    public int ParticipantCount { get; set; }
}
