namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendMetaNameMaxLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Albums", "IX_Name");
            DropIndex("dbo.Artists", new[] { "Name" });
            DropIndex("dbo.Tracks", "IX_Name");
            AlterColumn("dbo.Albums", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Tracks", "Name", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Albums", new[] { "Name", "ArtistId" }, unique: true, name: "IX_Name");
            CreateIndex("dbo.Artists", "Name", unique: true);
            CreateIndex("dbo.Tracks", new[] { "Name", "AlbumId" }, unique: true, name: "IX_Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", "IX_Name");
            DropIndex("dbo.Artists", new[] { "Name" });
            DropIndex("dbo.Albums", "IX_Name");
            AlterColumn("dbo.Tracks", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Albums", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Tracks", new[] { "Name", "AlbumId" }, unique: true, name: "IX_Name");
            CreateIndex("dbo.Artists", "Name", unique: true);
            CreateIndex("dbo.Albums", new[] { "Name", "ArtistId" }, unique: true, name: "IX_Name");
        }
    }
}
