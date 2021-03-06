namespace NashGaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gamers",
                c => new
                    {
                        GamerID = c.Int(nullable: false, identity: true),
                        Handle = c.String(),
                        Platform = c.String(),
                        Team_TeamID = c.Int(),
                        MemberOf_TeamID = c.Int(),
                        RealUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.GamerID)
                .ForeignKey("dbo.Teams", t => t.Team_TeamID)
                .ForeignKey("dbo.Teams", t => t.MemberOf_TeamID)
                .ForeignKey("dbo.AspNetUsers", t => t.RealUser_Id)
                .Index(t => t.Team_TeamID)
                .Index(t => t.MemberOf_TeamID)
                .Index(t => t.RealUser_Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Author_GamerID = c.Int(),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.Gamers", t => t.Author_GamerID)
                .Index(t => t.Author_GamerID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        TeamName = c.String(),
                        DateFounded = c.DateTime(nullable: false),
                        Rank = c.Int(nullable: false),
                        Website = c.String(),
                        Active = c.Boolean(nullable: false),
                        Founder_GamerID = c.Int(),
                    })
                .PrimaryKey(t => t.TeamID)
                .ForeignKey("dbo.Gamers", t => t.Founder_GamerID)
                .Index(t => t.Founder_GamerID);
            
            CreateTable(
                "dbo.Leagues",
                c => new
                    {
                        LeagueID = c.Int(nullable: false, identity: true),
                        LeagueName = c.String(),
                        Platform = c.String(),
                    })
                .PrimaryKey(t => t.LeagueID);
            
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Result = c.String(),
                        Team1Score = c.Int(nullable: false),
                        Team2Score = c.Int(nullable: false),
                        League_LeagueID = c.Int(),
                        Team1_TeamID = c.Int(nullable: false),
                        Team2_TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MatchID)
                .ForeignKey("dbo.Leagues", t => t.League_LeagueID)
                .ForeignKey("dbo.Teams", t => t.Team1_TeamID, cascadeDelete: false)
                .ForeignKey("dbo.Teams", t => t.Team2_TeamID, cascadeDelete: false)
                .Index(t => t.League_LeagueID)
                .Index(t => t.Team1_TeamID)
                .Index(t => t.Team2_TeamID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Handle = c.String(),
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
                "dbo.LeagueTeams",
                c => new
                    {
                        League_LeagueID = c.Int(nullable: false),
                        Team_TeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.League_LeagueID, t.Team_TeamID })
                .ForeignKey("dbo.Leagues", t => t.League_LeagueID, cascadeDelete: false)
                .ForeignKey("dbo.Teams", t => t.Team_TeamID, cascadeDelete: false)
                .Index(t => t.League_LeagueID)
                .Index(t => t.Team_TeamID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Gamers", "RealUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Gamers", "MemberOf_TeamID", "dbo.Teams");
            DropForeignKey("dbo.Gamers", "Team_TeamID", "dbo.Teams");
            DropForeignKey("dbo.LeagueTeams", "Team_TeamID", "dbo.Teams");
            DropForeignKey("dbo.LeagueTeams", "League_LeagueID", "dbo.Leagues");
            DropForeignKey("dbo.Matches", "Team2_TeamID", "dbo.Teams");
            DropForeignKey("dbo.Matches", "Team1_TeamID", "dbo.Teams");
            DropForeignKey("dbo.Matches", "League_LeagueID", "dbo.Leagues");
            DropForeignKey("dbo.Teams", "Founder_GamerID", "dbo.Gamers");
            DropForeignKey("dbo.Posts", "Author_GamerID", "dbo.Gamers");
            DropIndex("dbo.LeagueTeams", new[] { "Team_TeamID" });
            DropIndex("dbo.LeagueTeams", new[] { "League_LeagueID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Matches", new[] { "Team2_TeamID" });
            DropIndex("dbo.Matches", new[] { "Team1_TeamID" });
            DropIndex("dbo.Matches", new[] { "League_LeagueID" });
            DropIndex("dbo.Teams", new[] { "Founder_GamerID" });
            DropIndex("dbo.Posts", new[] { "Author_GamerID" });
            DropIndex("dbo.Gamers", new[] { "RealUser_Id" });
            DropIndex("dbo.Gamers", new[] { "MemberOf_TeamID" });
            DropIndex("dbo.Gamers", new[] { "Team_TeamID" });
            DropTable("dbo.LeagueTeams");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Matches");
            DropTable("dbo.Leagues");
            DropTable("dbo.Teams");
            DropTable("dbo.Posts");
            DropTable("dbo.Gamers");
        }
    }
}
