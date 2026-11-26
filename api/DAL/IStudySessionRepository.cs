using StudyFlow.Models;

namespace StudyFlow.DAL
{
    public interface IStudySessionRepository
    {
        Task<IEnumerable<StudySession>?> GetAll();

        Task<StudySession?> GetById(int id);

        Task<bool> CreateAsync(StudySession session);

        Task<bool> UpdateAsync(StudySession session);

        Task<bool> DeleteAsync(int id);

        Task<bool> AddParticipantAsync(StudySessionParticipant participant);

        Task<bool> DeleteParticipantAsync(String userId, StudySession session);

        // Returns null if the room does not exist.
        Task<int?> GetRoomCapacity(int roomId);
    }
}
