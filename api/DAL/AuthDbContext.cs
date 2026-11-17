using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyFlow.Models; 


namespace StudyFlow.DAL
{
    public class AuthDbContext : IdentityDbContext<AuthUser>{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {}
}
}