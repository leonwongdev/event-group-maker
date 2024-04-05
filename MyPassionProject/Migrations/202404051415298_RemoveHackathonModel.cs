namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveHackathonModel : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Hackathons");
        }
        
        public override void Down()
        {
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
            
        }
    }
}
