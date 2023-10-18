using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementSystem.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
