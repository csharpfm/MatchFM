namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        ReleaseDate = c.DateTime(),
                        ArtistId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        MbId = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .Index(t => new { t.Name, t.ArtistId }, unique: true, name: "IX_Name")
                .Index(t => t.MbId);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Name = c.String(nullable: false, maxLength: 200),
                        MbId = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.MbId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        MbId = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Duration = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        MbId = c.String(maxLength: 36),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => new { t.Name, t.AlbumId }, unique: true, name: "IX_Name")
                .Index(t => t.MbId);
            
            CreateTable(
                "dbo.UserTracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        TrackId = c.Int(nullable: false),
                        ListenDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tracks", t => t.TrackId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.TrackId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Gender = c.Int(nullable: false),
                        Photo = c.String(),
                        Location = c.Geography(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.TagAlbums",
                c => new
                    {
                        Tag_Name = c.String(nullable: false, maxLength: 128),
                        Album_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Name, t.Album_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Name, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Tag_Name)
                .Index(t => t.Album_Id);
            
            CreateTable(
                "dbo.TagArtists",
                c => new
                    {
                        Tag_Name = c.String(nullable: false, maxLength: 128),
                        Artist_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Name, t.Artist_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Name, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_Id, cascadeDelete: true)
                .Index(t => t.Tag_Name)
                .Index(t => t.Artist_Id);
            
            CreateTable(
                "dbo.TagTracks",
                c => new
                    {
                        Tag_Name = c.String(nullable: false, maxLength: 128),
                        Track_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Name, t.Track_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Name, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.Track_Id, cascadeDelete: true)
                .Index(t => t.Tag_Name)
                .Index(t => t.Track_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TagTracks", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.TagTracks", "Tag_Name", "dbo.Tags");
            DropForeignKey("dbo.UserTracks", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTracks", "TrackId", "dbo.Tracks");
            DropForeignKey("dbo.Tracks", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.TagArtists", "Artist_Id", "dbo.Artists");
            DropForeignKey("dbo.TagArtists", "Tag_Name", "dbo.Tags");
            DropForeignKey("dbo.TagAlbums", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.TagAlbums", "Tag_Name", "dbo.Tags");
            DropForeignKey("dbo.Albums", "ArtistId", "dbo.Artists");
            DropIndex("dbo.TagTracks", new[] { "Track_Id" });
            DropIndex("dbo.TagTracks", new[] { "Tag_Name" });
            DropIndex("dbo.TagArtists", new[] { "Artist_Id" });
            DropIndex("dbo.TagArtists", new[] { "Tag_Name" });
            DropIndex("dbo.TagAlbums", new[] { "Album_Id" });
            DropIndex("dbo.TagAlbums", new[] { "Tag_Name" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserTracks", new[] { "TrackId" });
            DropIndex("dbo.UserTracks", new[] { "UserId" });
            DropIndex("dbo.Tracks", new[] { "MbId" });
            DropIndex("dbo.Tracks", "IX_Name");
            DropIndex("dbo.Artists", new[] { "MbId" });
            DropIndex("dbo.Artists", new[] { "Name" });
            DropIndex("dbo.Albums", new[] { "MbId" });
            DropIndex("dbo.Albums", "IX_Name");
            DropTable("dbo.TagTracks");
            DropTable("dbo.TagArtists");
            DropTable("dbo.TagAlbums");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserTracks");
            DropTable("dbo.Tracks");
            DropTable("dbo.Tags");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}
