using Microsoft.EntityFrameworkCore;
using mssql_cassandra_benchmark.src.Data.Entity;

namespace mssql_cassandra_benchmark.src.Data
{
    public class CodePoolStore : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=CodePool;User Id=sa; Password=Keyboard_cat1234; TrustServerCertificate=true;");
        }

        public DbSet<CodePoolCollection> CodePoolCollection { get; set; }
    }
}
