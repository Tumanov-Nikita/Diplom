using System.Data.Entity;

namespace DIPLOM.Model
{
    public class DB_Context : DbContext
        {
            public DB_Context() : base("DbConnection")
            {

            }
            public DbSet<AutoPart> AutoParts { get; set; }

            public DbSet<Car> Cars { get; set; }

            public DbSet<Complectation> Complectations { get; set; }

            public DbSet<Order> Orders { get; set; }

            public DbSet<Producer> Producers { get; set; }

            public DbSet<SelectedAutoPart> SelectedAutoParts { get; set; }

            public DbSet<User> Users { get; set; }

            public DbSet<Group> Groups { get; set; }

            public DbSet<Compatibility> Compatibilities { get; set; }
    }
}
