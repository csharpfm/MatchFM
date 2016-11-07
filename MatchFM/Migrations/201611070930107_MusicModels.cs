namespace MatchFM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MusicModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MbId = c.String(maxLength: 36),
                        Image = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .Index(t => t.MbId, unique: true, name: "AlbumMbID")
                .Index(t => t.ArtistId);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MbId = c.String(maxLength: 36),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.MbId, unique: true, name: "ArtistMbID");
            
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
                        Name = c.String(nullable: false),
                        MbId = c.String(maxLength: 36),
                        Duration = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.MbId, unique: true, name: "TrackMbID")
                .Index(t => t.AlbumId);
            
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
            DropForeignKey("dbo.TagTracks", "Track_Id", "dbo.Tracks");
            DropForeignKey("dbo.TagTracks", "Tag_Name", "dbo.Tags");
            DropForeignKey("dbo.UserTracks", "UserId", "dbo.AspNetUsers");
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
            DropIndex("dbo.UserTracks", new[] { "TrackId" });
            DropIndex("dbo.UserTracks", new[] { "UserId" });
            DropIndex("dbo.Tracks", new[] { "AlbumId" });
            DropIndex("dbo.Tracks", "TrackMbID");
            DropIndex("dbo.Artists", "ArtistMbID");
            DropIndex("dbo.Albums", new[] { "ArtistId" });
            DropIndex("dbo.Albums", "AlbumMbID");
            DropTable("dbo.TagTracks");
            DropTable("dbo.TagArtists");
            DropTable("dbo.TagAlbums");
            DropTable("dbo.UserTracks");
            DropTable("dbo.Tracks");
            DropTable("dbo.Tags");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}
