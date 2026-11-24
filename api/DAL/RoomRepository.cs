using Microsoft.EntityFrameworkCore;
using StudyFlow.Models;

namespace StudyFlow.DAL;

public class RoomRepository : IRoomRepository
{
    private readonly StudyFlowDbContext _context;

    public RoomRepository(StudyFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAll()
    {
        return await _context.Rooms
            .AsNoTracking()
            .OrderBy(room => room.Name)
            .ToListAsync();
    }

    public async Task<Room?> GetById(int id)
    {
        return await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(room => room.Id == id);
    }
}