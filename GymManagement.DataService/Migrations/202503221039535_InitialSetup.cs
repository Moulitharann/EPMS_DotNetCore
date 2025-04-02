namespace GymManagement.DataService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetup : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Customers", newName: "CustomerDBs");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CustomerDBs", newName: "Customers");
        }
    }
}
