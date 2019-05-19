using Microsoft.EntityFrameworkCore;

namespace mymvc_core.Models
{
    public partial class CoremvcContext : DbContext
    {
        public CoremvcContext()
        {
        }

        public CoremvcContext(DbContextOptions<CoremvcContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Messages> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseMySql(@"server=localhost;port=3306;user=root;password=root;database=Coremvc");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
