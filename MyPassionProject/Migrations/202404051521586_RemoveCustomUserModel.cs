namespace MyPassionProject.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveCustomUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppUserEvents", "AppUser_UserId", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserEvents", "Event_EventId", "dbo.Events");
            DropIndex("dbo.AppUserEvents", new[] { "AppUser_UserId" });
            DropIndex("dbo.AppUserEvents", new[] { "Event_EventId" });
            DropTable("dbo.AppUserEvents");
            DropTable("dbo.AppUsers");

        }

        public override void Down()
        {
            CreateTable(
                "dbo.AppUserEvents",
                c => new
                {
                    AppUser_UserId = c.Int(nullable: false),
                    Event_EventId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.AppUser_UserId, t.Event_EventId });

            CreateTable(
                "dbo.AppUsers",
                c => new
                {
                    UserId = c.Int(nullable: false, identity: true),
                    UserName = c.String(),
                    Password = c.String(),
                    Email = c.String(),
                    PhoneNumber = c.String(),
                })
                .PrimaryKey(t => t.UserId);

            CreateIndex("dbo.AppUserEvents", "Event_EventId");
            CreateIndex("dbo.AppUserEvents", "AppUser_UserId");
            AddForeignKey("dbo.AppUserEvents", "Event_EventId", "dbo.Events", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.AppUserEvents", "AppUser_UserId", "dbo.AppUsers", "UserId", cascadeDelete: true);
        }
    }
}
