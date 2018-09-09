namespace ANPositive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AteamComTrDb2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Galleries", "Images", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Galleries", "Images");
        }
    }
}
