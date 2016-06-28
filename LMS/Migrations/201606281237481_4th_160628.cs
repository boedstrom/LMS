namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4th_160628 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserViewModels", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserViewModels", "UserId");
        }
    }
}
