using Microsoft.EntityFrameworkCore;

namespace demoDotnet.Models {
  public class DemoDotnetContext : DbContext {
    public DemoDotnetContext (DbContextOptions<DemoDotnetContext> options) : base (options) { }
    public DbSet<Student> Students { get; set; }

    public DbSet<Classroom> Classrooms { get; set; }
  }
}