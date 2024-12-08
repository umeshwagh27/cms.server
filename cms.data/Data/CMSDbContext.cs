using cms.data.Model;
using Microsoft.EntityFrameworkCore;
namespace cms.data.Data
{
    public partial class CMSDbContext : DbContext
    {
        public CMSDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<CarModelClass> CarModels { get; set; }
    }
}
