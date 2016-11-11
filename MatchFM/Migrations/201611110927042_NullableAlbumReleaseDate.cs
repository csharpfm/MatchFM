namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableAlbumReleaseDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Albums", "ReleaseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Albums", "ReleaseDate", c => c.DateTime(nullable: false));
        }
    }
}
