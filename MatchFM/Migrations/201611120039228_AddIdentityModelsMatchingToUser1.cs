namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentityModelsMatchingToUser1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentityModelsMatchings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ProfilId = c.String(nullable: false, maxLength: 128),
                    Match = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ProfilId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProfilId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.IdentityModelsMatchings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IdentityModelsMatchings", "ProfilId", "dbo.AspNetUsers");
            DropIndex("dbo.IdentityModelsMatchings", new[] { "ProfilId" });
            DropIndex("dbo.IdentityModelsMatchings", new[] { "UserId" });
            DropTable("dbo.IdentityModelsMatchings");
        }
    }
}
