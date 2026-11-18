using StudyFlow.Models;

namespace StudyFlow.DAL
{
    public interface IBookingRepository
    {
        Task<bool> HasOverlapAsync(
            int roomId,
            DateTime startTime,
            DateTime endTime,
            int? excludeBookingId = null);



        Task<IEnumerable<Booking>?> GetAll(); 

        Task<Booking?> GetById(int id); 

        Task<bool> CreateAsync(Booking booking);

        Task<bool> UpdateAsync(Booking booking);

        Task<bool> DeleteAsync(int id);
    }
}
