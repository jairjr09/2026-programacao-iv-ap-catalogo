using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using umfgcloud.infraestrutura.service.Extensions;
using umfgcloud.infraestrutura.service.Maps;

namespace umfgcloud.infraestrutura.service.Context
{
    public sealed class MySqlDataBaseContext : IdentityDbContext
    {
        public MySqlDataBaseContext(DbContextOptions<MySqlDataBaseContext> options) 
            : base(options)
        {
            ApplyMigrations();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureToMySql();

            builder.ApplyConfiguration(new ProdutoMap());
        }

        private void ApplyMigrations()
        {
            if (Database.GetPendingMigrations().Any())
                Database.Migrate();
        }
    }
}
