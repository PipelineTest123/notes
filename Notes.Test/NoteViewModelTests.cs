using Xunit;
using Notes.Database.Models;

namespace Notes.Test;

public class NoteViewModelTests
{
    [Fact]
    public void Save_NewNote_ShouldCreateDatabaseRecord()
    {

        // Arrange
        var note = new Note();
        note.Date = DateTime.Now;
        note.Text = "I am a test note";


        // Act

        // Assert
    }
}
