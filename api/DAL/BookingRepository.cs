using Microsoft.EntityFrameworkCore;
using StudyFlow.Models;

namespace StudyFlow.DAL
{
    public class BookingRepository : IBookingRepository
    {
        private readonly StudyFlowDbContext _context;

        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(StudyFlowDbContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // excludeBookingId lets an update ignore the booking being edited,
        // so it doesn't count as overlapping with itself.
        public async Task<bool> HasOverlapAsync(
            int roomId,
            DateTime startTime,
            DateTime endTime,
            int? excludeBookingId = null)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.RoomId == roomId &&
                (excludeBookingId == null || b.Id != excludeBookingId) &&
                b.StartTime < endTime &&
                startTime < b.EndTime);
        }

        public async Task<IEnumerable<Booking>?> GetAll()
        {
            try
            {
                return await _context.Bookings.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[BookingRepository] Bookings to list failed with getAll(), error message: {e}", e.Message); 

                return null;
            }
        }

        public async Task<bool> CreateAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return true; // Id is populated after SaveChanges
        }

        // Returns false if the booking does not exist.
        public async Task<bool> UpdateAsync(Booking booking)
        {
            var existing = await _context.Bookings.FindAsync(booking.Id);
            if (existing == null)
            {
                return false;
            }

            // Only copy editable fields; UserId and CreatedAt stay unchanged.
            existing.RoomId = booking.RoomId;
            existing.StartTime = booking.StartTime;
            existing.EndTime = booking.EndTime;
            existing.Subject = booking.Subject;

            await _context.SaveChangesAsync();
            return true;
        }

        // Returns false if the booking does not exist.
        public async Task<bool> DeleteAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            //Check booking exists
            if (booking == null)
            {
                return false;
            }

            //Remove Booking
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
