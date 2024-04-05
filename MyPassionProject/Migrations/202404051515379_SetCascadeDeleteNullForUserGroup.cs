namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetCascadeDeleteNullForUserGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.AspNetUsers");
            AddForeignKey("dbo.ApplicationUserGroups", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
