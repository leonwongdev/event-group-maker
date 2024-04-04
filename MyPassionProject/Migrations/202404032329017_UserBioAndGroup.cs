namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBioAndGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserGroups",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        HackathonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.HackathonId })
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .ForeignKey("dbo.Hackathons", t => t.HackathonId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId)
                .Index(t => t.HackathonId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HackathonId = c.Int(nullable: false),
                        TeamLeaderId = c.String(),
                        Requirements = c.String(maxLength: 300),
                        MaxNumOfMembers = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hackathons", t => t.HackathonId, cascadeDelete: true)
                .Index(t => t.HackathonId);
            
            CreateTable(
                "dbo.Hackathons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Location = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false, maxLength: 300),
                        Url = c.String(nullable: false, maxLength: 255),
                        Deadline = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Bio", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserGroups", "HackathonId", "dbo.Hackathons");
            DropForeignKey("dbo.ApplicationUserGroups", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Groups", "HackathonId", "dbo.Hackathons");
            DropIndex("dbo.Groups", new[] { "HackathonId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "HackathonId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "GroupId" });
            DropIndex("dbo.ApplicationUserGroups", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "Bio");
            DropTable("dbo.Hackathons");
            DropTable("dbo.Groups");
            DropTable("dbo.ApplicationUserGroups");
        }
    }
}
