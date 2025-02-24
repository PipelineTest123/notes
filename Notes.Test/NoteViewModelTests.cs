using Xunit;
using Notes.Database.Models;

namespace Notes.Test;

public class NoteViewModelTests : IClassFixture<DatabaseFixture>
{
    DatabaseFixture _fixture;
    public NoteViewModelTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _fixture.Seed();
    }

    [Fact]
    public void Save_NewNote_ShouldCreateDatabaseRecord()
    {

        // Arrange
        var note = new Note();
        note.Date = DateTime.Now;
        note.Text = "I am a test note";
        note.ProjectId = 1;


        // Act
        _fixture._testDbContext.Add(note);
        _fixture._testDbContext.SaveChanges();


        // Assert
        Assert.NotEqual(note.Id, 0);

    }
}
