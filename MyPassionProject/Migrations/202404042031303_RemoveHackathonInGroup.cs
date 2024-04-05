namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveHackathonInGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Groups", "HackathonId", "dbo.Hackathons");
            DropForeignKey("dbo.ApplicationUserGroups", "HackathonId", "dbo.Hackathons");
            DropIndex("dbo.ApplicationUserGroups", new[] { "HackathonId" });
            DropIndex("dbo.Groups", new[] { "HackathonId" });
            DropPrimaryKey("dbo.ApplicationUserGroups");
            AddColumn("dbo.ApplicationUserGroups", "EventId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "UserId", "EventId" });
            CreateIndex("dbo.ApplicationUserGroups", "EventId");
            AddForeignKey("dbo.ApplicationUserGroups", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
            DropColumn("dbo.ApplicationUserGroups", "HackathonId");
            DropColumn("dbo.Groups", "HackathonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "HackathonId", c => c.Int(nullable: false));
            AddColumn("dbo.ApplicationUserGroups", "HackathonId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ApplicationUserGroups", "EventId", "dbo.Events");
            DropIndex("dbo.ApplicationUserGroups", new[] { "EventId" });
            DropPrimaryKey("dbo.ApplicationUserGroups");
            DropColumn("dbo.ApplicationUserGroups", "EventId");
            AddPrimaryKey("dbo.ApplicationUserGroups", new[] { "UserId", "HackathonId" });
            CreateIndex("dbo.Groups", "HackathonId");
            CreateIndex("dbo.ApplicationUserGroups", "HackathonId");
            AddForeignKey("dbo.ApplicationUserGroups", "HackathonId", "dbo.Hackathons", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Groups", "HackathonId", "dbo.Hackathons", "Id", cascadeDelete: true);
        }
    }
}
