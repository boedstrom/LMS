namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newUser2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserViewModels", "AddUserViewModel_Id", "dbo.AddUserViewModels");
            DropIndex("dbo.UserViewModels", new[] { "AddUserViewModel_Id" });
            DropColumn("dbo.UserViewModels", "AddUserViewModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserViewModels", "AddUserViewModel_Id", c => c.Int());
            CreateIndex("dbo.UserViewModels", "AddUserViewModel_Id");
            AddForeignKey("dbo.UserViewModels", "AddUserViewModel_Id", "dbo.AddUserViewModels", "Id");
        }
    }
}
