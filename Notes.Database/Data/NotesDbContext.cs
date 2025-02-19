using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notes.Database.Models;

namespace Notes.Database.Data;

public class NotesDbContext : DbContext
{

    public NotesDbContext()
    { }
    public NotesDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Project> Projects { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = Assembly.GetExecutingAssembly();
        var resources = a.GetManifestResourceNames();
        using var stream = a.GetManifestResourceStream("Notes.Database.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString("DevelopmentConnection"),
            m => m.MigrationsAssembly("Notes.Migrations"));
    }

}
