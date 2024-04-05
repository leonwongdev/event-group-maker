namespace MyPassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMaxNumMemberFieldFromGroup : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Groups", "MaxNumOfMembers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "MaxNumOfMembers", c => c.Int(nullable: false));
        }
    }
}
