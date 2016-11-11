namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class AddLocationToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Location", c => c.Geography());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Location");
        }
    }
}
