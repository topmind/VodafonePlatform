namespace VodafoneWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_changehistory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvetoryChangeHistories", "OldOperationType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvetoryChangeHistories", "OldOperationType", c => c.Int(nullable: false));
        }
    }
}
