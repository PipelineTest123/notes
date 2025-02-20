using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notes.Database.Models;

namespace Notes.Database.Data;

public abstract class GenericDbContext : DbContext
{

    public GenericDbContext()
    { }
    public GenericDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Project> Projects { get; set; }

    internal abstract string connectionName { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var a = Assembly.GetExecutingAssembly();
        var resources = a.GetManifestResourceNames();
        using var stream = a.GetManifestResourceStream("Notes.Database.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        optionsBuilder.UseSqlServer(
            config.GetConnectionString(connectionName),
            m => m.MigrationsAssembly("Notes.Migrations"));
    }

}

public class NotesDbContext : GenericDbContext
{
    internal override String connectionName { get; set; } = "DevelopmentConnection";
}

public class TestDbContext : GenericDbContext
{
    internal override String connectionName { get; set; } = "TestConnection";
}
