namespace StudyFlow.DTOs;

public class StudySessionDto
{
    public int RoomId { get; set; }

    public string Course { get; set; } = string.Empty;

    public string Topic { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsPublic { get; set; }
}