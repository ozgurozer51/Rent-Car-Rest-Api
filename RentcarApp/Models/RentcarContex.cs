using Microsoft.EntityFrameworkCore;


namespace RentcarApp.Models
{
    public class RentcarContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public RentcarContext(DbContextOptions<RentcarContext> options) : base(options)
    {

    }
}
    
}
