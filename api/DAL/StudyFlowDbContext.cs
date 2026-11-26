using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyFlow.Models;


namespace StudyFlow.DAL
{
    public class StudyFlowDbContext : DbContext
    {
        public StudyFlowDbContext(DbContextOptions<StudyFlowDbContext> options) : base(options)
        {
            /* Study Flow DB Context 
            Handles:
            - Booking
            - Study Sessions
            - Rooms
            */
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            //Convert all times to UTC 
            builder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>(); 
        }

        private class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
        { // Helper function for DB to DTO conversion
                public UtcDateTimeConverter(): base(
                toDb => toDb.ToUniversalTime(),
                fromDb => DateTime.SpecifyKind(fromDb, DateTimeKind.Utc))
                {}
        }

        

        public DbSet<Booking> Bookings {get; set;}
        public DbSet<StudySession> StudySessions { get; set; }

        public DbSet<StudySessionParticipant> StudySessionParticipants { get; set; }

        
        public DbSet<Room> Rooms {get; set;}
        
        }

}