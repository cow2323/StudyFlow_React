using System.ComponentModel.DataAnnotations;

namespace StudyFlow.DTOs;

public class StudySessionDto : BookingDto
{

    [Required]
    [StringLength(100)]
    public string Course { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Topic { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }


    public bool IsPublic { get; set; } = true;

    // Read-only info for the client. Ignored on create/edit.
    public int ParticipantCount { get; set; }
}
