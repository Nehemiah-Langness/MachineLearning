using Data.Services;
using Microsoft.EntityFrameworkCore;

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
    }
}
