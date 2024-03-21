using Microsoft.EntityFrameworkCore;

namespace RabbitMQProducer
{
        public class OrderDbContext : DbContext
        {
            public DbSet<Order> Orders { get; set; }

            public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
            {
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasKey(a => a.Id);

        }
    }
}