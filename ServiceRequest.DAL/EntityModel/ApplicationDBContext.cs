using Microsoft.EntityFrameworkCore;

namespace ServiceRequest.DAL.EntityModel
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<ServiceRequestEntity> ServiceRequest { get; set; }
    }
}
