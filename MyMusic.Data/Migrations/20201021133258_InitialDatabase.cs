using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMusic.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Linkin Park')");
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Iron Maiden')");
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Flogging Molly')");
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Red Hot Chilli Peppers')");

            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('In The End', (SELECT Id FROM Artist WHERE Name = 'Linkin Park'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Numb', (SELECT Id FROM Artist WHERE Name = 'Linkin Park'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Breaking The Habit', (SELECT Id FROM Artist WHERE Name = 'Linkin Park'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Fear of the dark', (SELECT Id FROM Artist WHERE Name = 'Iron Maiden'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Number of the beast', (SELECT Id FROM Artist WHERE Name = 'Iron Maiden'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('The Trooper', (SELECT Id FROM Artist WHERE Name = 'Iron Maiden'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('What''s left of the flag', (SELECT Id FROM Artist WHERE Name = 'Flogging Molly'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Drunken Lullabies', (SELECT Id FROM Artist WHERE Name = 'Flogging Molly'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('If I Ever Leave this World Alive', (SELECT Id FROM Artist WHERE Name = 'Flogging Molly'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Californication', (SELECT Id FROM Artist WHERE Name = 'Red Hot Chilli Peppers'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Tell Me Baby', (SELECT Id FROM Artist WHERE Name = 'Red Hot Chilli Peppers'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, IdArtist) Values ('Parallel Universe', (SELECT Id FROM Artist WHERE Name = 'Red Hot Chilli Peppers'))");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
            .Sql("DELETE FROM Music");

            migrationBuilder
                .Sql("DELETE FROM Artist");

        }
    }
}
