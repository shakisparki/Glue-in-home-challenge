using Microsoft.EntityFrameworkCore;
using TT.Deliveries.Data.Entities;

namespace TT.Deliveries.Data
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
        {
        }

        public DbSet<Delivery> Delivery { get; set; }
    }
}
