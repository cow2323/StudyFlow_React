namespace StudyFlow.Models
{
    public class StudySession : Booking
    {
       
        public string Course { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string? Description { get; set; }
         
        public bool IsPublic { get; set; } = true;

        public List<StudySessionParticipant> Participants {get; set;} = new List<StudySessionParticipant>();
        

    }
}



