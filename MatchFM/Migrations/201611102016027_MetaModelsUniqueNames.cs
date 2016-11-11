namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetaModelsUniqueNames : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Albums", new[] { "ArtistId" });
            DropIndex("dbo.Tracks", new[] { "AlbumId" });
            AlterColumn("dbo.Albums", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tracks", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Albums", new[] { "Name", "ArtistId" }, unique: true, name: "IX_Name");
            CreateIndex("dbo.Artists", "Name", unique: true);
            CreateIndex("dbo.Tracks", new[] { "Name", "AlbumId" }, unique: true, name: "IX_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", "IX_Name");
            DropIndex("dbo.Artists", new[] { "Name" });
            DropIndex("dbo.Albums", "IX_Name");
            AlterColumn("dbo.Tracks", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Albums", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Tracks", "AlbumId");
            CreateIndex("dbo.Albums", "ArtistId");
        }
    }
}
