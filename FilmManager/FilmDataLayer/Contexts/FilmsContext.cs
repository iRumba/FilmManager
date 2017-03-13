using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer.Models;
using FilmDataLayer.SqliteUtils;

namespace FilmDataLayer.Contexts
{
    class FilmsContext : DbContext
    {
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }

        public FilmsContext(string connectionString) : base(new SQLiteConnection(connectionString), true)
        {
            // Проверка на существование базы. По структуре проверять не стал, думаю, это лишнее
            var cb = new SQLiteConnectionStringBuilder(Database.Connection.ConnectionString);
            if (!File.Exists(cb.DataSource) || File.OpenRead(cb.DataSource).Length == 0)
            {
                File.WriteAllBytes(cb.DataSource, Properties.Resources.DB_films_default);
            }
            SQLiteFunction.RegisterFunction(typeof(SqliteCharindexFunction));
            SQLiteFunction.RegisterFunction(typeof(SqliteLowerFunction));
            SQLiteFunction.RegisterFunction(typeof(SqliteUpperFunction));
            //Database.SetInitializer<FilmsContext>(null);

            //var conf = new SqliteDbConfiguration();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Film>().HasMany(g => g.Genres).WithMany(f => f.Films).Map(m =>
            {
                m.ToTable("FilmGenres");
                m.MapLeftKey("FilmId");
                m.MapRightKey("GenreId");
            });
            modelBuilder.Entity<Film>().Property(f => f.AddingDate).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);
        }
    }
}
