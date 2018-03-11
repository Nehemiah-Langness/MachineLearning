using Data.Services;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Persistance
{
    internal class Context : DbContext
    {
        private readonly string _databaseName;

        public Context() : this(Connections.Test) { }
        public Context(string database)
        {
            _databaseName = database;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databaseName}.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRules(modelBuilder.Entity<Rule>());
        }


        private static void ConfigureRules(EntityTypeBuilder<Rule> rules)
        {
            rules.HasMany(e => e.Conditions)
                .WithOne(e => e.Rule)
                .IsRequired();

            rules.HasMany(e => e.Outcomes)
                .WithOne(e => e.Rule)
                .IsRequired();
        }
    }
}
