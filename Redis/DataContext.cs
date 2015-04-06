using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("Connection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Entity> Entidades { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DataContext>(null);

            modelBuilder.Entity<Entity>().ToTable(Entity.TABLE_NAME);
        }
    }
}
