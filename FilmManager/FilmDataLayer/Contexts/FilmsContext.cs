using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer.Models;

namespace FilmDataLayer.Contexts
{
    class FilmsContext : DbContext
    {
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }

        public FilmsContext(string connectionString) : base(new SQLiteConnection(connectionString), true)
        {
            var conf = new SqliteDbConfiguration();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
