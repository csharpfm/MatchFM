namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropMbIdUniquenessConstraint : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Albums", new[] { "MbId" });
            DropIndex("dbo.Artists", new[] { "MbId" });
            DropIndex("dbo.Tracks", new[] { "MbId" });
            CreateIndex("dbo.Albums", "MbId");
            CreateIndex("dbo.Artists", "MbId");
            CreateIndex("dbo.Tracks", "MbId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", new[] { "MbId" });
            DropIndex("dbo.Artists", new[] { "MbId" });
            DropIndex("dbo.Albums", new[] { "MbId" });
            CreateIndex("dbo.Tracks", "MbId", unique: true);
            CreateIndex("dbo.Artists", "MbId", unique: true);
            CreateIndex("dbo.Albums", "MbId", unique: true);
        }
    }
}
