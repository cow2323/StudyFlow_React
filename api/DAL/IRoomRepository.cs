using StudyFlow.Models;

namespace StudyFlow.DAL;

public interface IRoomRepository
{
    Task<List<Room>> GetAll();
    Task<Room?> GetById(int id);
}