using Microsoft.EntityFrameworkCore;
using StudyFlow.Models;

namespace StudyFlow.DAL
{
    public class StudySessionRepository : IStudySessionRepository
    {
        private readonly StudyFlowDbContext _context;
        private readonly ILogger<StudySessionRepository> _logger;

        public StudySessionRepository(StudyFlowDbContext context, ILogger<StudySessionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<StudySession>?> GetAll()
        {
            try
            {
                return await _context.StudySessions
                    .Include(s => s.Participants)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] GetAll() failed, error message: {Error}", e.Message);
                return null;
            }
        }

        // Returns null if the session does not exist.
        public async Task<StudySession?> GetById(int id)
        {
            try
            {
                return await _context.StudySessions
                    .Include(s => s.Participants)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] GetById({Id}) failed, error message: {Error}", id, e.Message);
                return null;
            }
        }

        public async Task<bool> CreateAsync(StudySession session)
        {
            try
            {
                // Participants added to session.Participants are saved together with the session.
                _context.StudySessions.Add(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] CreateAsync failed, error message: {Error}", e.Message);
                return false;
            }
        }

        // The session must be loaded with GetById first; EF tracks the changes.
        public async Task<bool> UpdateAsync(StudySession session)
        {
            try
            {
                _context.StudySessions.Update(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] UpdateAsync({Id}) failed, error message: {Error}", session.Id, e.Message);
                return false;
            }
        }

        // Returns false if the session does not exist.
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var session = await _context.StudySessions
                    .Include(s => s.Participants)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (session == null)
                {
                    return false;
                }

                _context.StudySessionParticipants.RemoveRange(session.Participants);
                _context.StudySessions.Remove(session);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] DeleteAsync({Id}) failed, error message: {Error}", id, e.Message);
                return false;
            }
        }

        public async Task<bool> AddParticipantAsync(StudySessionParticipant participant)
        {
            try
            {
                _context.StudySessionParticipants.Add(participant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] AddParticipantAsync failed, error message: {Error}", e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteParticipantAsync(String userId, StudySession session)
        {
            var participant = await _context.StudySessionParticipants.FirstOrDefaultAsync(sP => (userId == sP.UserId && sP.Session == session));


            try
            {
                _context.StudySessionParticipants.Remove(participant);
                await _context.SaveChangesAsync(); 
                return true;
            }

            catch (Exception e)
            {
                _logger.LogError("[StudySessionRepository] DeleyeParticipantAsync failed, error message: {Error}", e.Message);
                return false;
            }

        }

        public async Task<int?> GetRoomCapacity(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            return room?.Capacity;
        }
    }
}
