namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataInheritance : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.Albums", name: "AlbumMbID", newName: "IX_MbId");
            RenameIndex(table: "dbo.Artists", name: "ArtistMbID", newName: "IX_MbId");
            RenameIndex(table: "dbo.Tracks", name: "TrackMbID", newName: "IX_MbId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tracks", name: "IX_MbId", newName: "TrackMbID");
            RenameIndex(table: "dbo.Artists", name: "IX_MbId", newName: "ArtistMbID");
            RenameIndex(table: "dbo.Albums", name: "IX_MbId", newName: "AlbumMbID");
        }
    }
}
