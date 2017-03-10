using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Linq;
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
            SQLiteFunction.RegisterFunction(typeof(SqliteCharindexFunction));
            SQLiteFunction.RegisterFunction(typeof(SqliteLowerFunction));
            SQLiteFunction.RegisterFunction(typeof(SqliteUpperFunction));
            Database.SetInitializer<FilmsContext>(null);
            var conf = new SqliteDbConfiguration();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //SQLiteFunction.RegisterFunction(typeof(SQLiteCaseInsensitiveCollation));
            //SQLiteFunction.RegisterFunction(typeof(SqLiteCyrHelper));
            modelBuilder.Entity<Film>().HasMany(g => g.Genres).WithMany(f => f.Films).Map(m =>
            {
                m.ToTable("FilmGenres");
                m.MapLeftKey("FilmId");
                m.MapRightKey("GenreId");
            });
            modelBuilder.Entity<Film>().Property(f => f.AddingDate).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            //modelBuilder.Entity<Film>().HasRequired(f => f.LocalName);
            //modelBuilder.Entity<Film>().Property(f => f.FilmId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Genre>().Property(g => g.GenreId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            //modelBuilder.Entity<Genre>().Property(g => g.Name).(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
        }
    }
}
