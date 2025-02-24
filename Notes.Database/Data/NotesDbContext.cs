using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notes.Database.Models;

namespace Notes.Database.Data;

public abstract class GenericDbContext : DbContext
{
    public GenericDbContext() { }
    public GenericDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Project> Projects { get; set; }

    internal abstract string ConnectionName { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 1. Get connection string from environment variable (GitHub Actions)
        var connectionString = Environment.GetEnvironmentVariable($"ConnectionStrings__{ConnectionName}");

        if (string.IsNullOrEmpty(connectionString))
        {
            // 2. Fallback to appsettings.json (local)
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("Notes.Database.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                connectionString = config.GetConnectionString(ConnectionName);
            }
        }

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is not configured.");
        }

        // 3. Use the connection string
        optionsBuilder.UseSqlServer(
            connectionString,
            m => m.MigrationsAssembly("Notes.Migrations"));
    }
}

public class NotesDbContext : GenericDbContext
{
    internal override string ConnectionName { get; set; } = "DevelopmentConnection";
}

public class TestDbContext : GenericDbContext
{
    internal override string ConnectionName { get; set; } = "TestConnection";
}
