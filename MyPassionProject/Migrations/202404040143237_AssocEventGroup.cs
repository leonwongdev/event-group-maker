namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssocEventGroup : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventAppUsers", newName: "AppUserEvents");
            DropPrimaryKey("dbo.AppUserEvents");
            AddColumn("dbo.Groups", "EventId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.AppUserEvents", new[] { "AppUser_UserId", "Event_EventId" });
            CreateIndex("dbo.Groups", "EventId");
            AddForeignKey("dbo.Groups", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "EventId", "dbo.Events");
            DropIndex("dbo.Groups", new[] { "EventId" });
            DropPrimaryKey("dbo.AppUserEvents");
            DropColumn("dbo.Groups", "EventId");
            AddPrimaryKey("dbo.AppUserEvents", new[] { "Event_EventId", "AppUser_UserId" });
            RenameTable(name: "dbo.AppUserEvents", newName: "EventAppUsers");
        }
    }
}
