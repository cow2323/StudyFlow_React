using System.ComponentModel.DataAnnotations;

namespace StudyFlow.DTOs;

public class RoomDto
{
    // Id is included so the React client can reference a room (e.g. when booking).
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Capacity must be 1 or greater")]
    public int Capacity { get; set; }

    // Optional, same as on the Room model.
    public string? Description { get; set; }
}
