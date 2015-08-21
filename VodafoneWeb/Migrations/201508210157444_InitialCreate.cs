namespace VodafoneWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        PlanId = c.Int(nullable: false, identity: true),
                        PlanName = c.String(),
                        OfferCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlanId)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        DealerId = c.Int(nullable: false, identity: true),
                        DealerName = c.String(),
                        DealerCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DealerId);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IMEI = c.String(maxLength: 450),
                        StockInDate = c.DateTime(nullable: false),
                        StockOutDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Dealer_DealerId = c.Int(),
                        Product_ID = c.Int(),
                        StockInBy_Id = c.String(maxLength: 128),
                        StockOutBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dealers", t => t.Dealer_DealerId)
                .ForeignKey("dbo.Products", t => t.Product_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.StockInBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.StockOutBy_Id)
                .Index(t => t.IMEI, unique: true)
                .Index(t => t.Dealer_DealerId)
                .Index(t => t.Product_ID)
                .Index(t => t.StockInBy_Id)
                .Index(t => t.StockOutBy_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SalesTransactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MobileNumber = c.String(),
                        Pin = c.String(),
                        OrderNumber = c.String(),
                        PortinNumber = c.String(),
                        PlanId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DealerId = c.Int(nullable: false),
                        InventoryId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        RefferA = c.String(),
                        RefferB = c.String(),
                        Gift = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dealers", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PlanId)
                .Index(t => t.UserId)
                .Index(t => t.DealerId)
                .Index(t => t.InventoryId);
            
            CreateTable(
                "dbo.InvetoryChangeHistories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ChangeDate = c.DateTime(nullable: false),
                        OperationType = c.Int(nullable: false),
                        OperatedByEmployeeID = c.String(),
                        Inventory_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Inventories", t => t.Inventory_ID)
                .Index(t => t.Inventory_ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.InvetoryChangeHistories", "Inventory_ID", "dbo.Inventories");
            DropForeignKey("dbo.SalesTransactions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesTransactions", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.SalesTransactions", "InventoryId", "dbo.Inventories");
            DropForeignKey("dbo.SalesTransactions", "DealerId", "dbo.Dealers");
            DropForeignKey("dbo.Inventories", "StockOutBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inventories", "StockInBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inventories", "Product_ID", "dbo.Products");
            DropForeignKey("dbo.Inventories", "Dealer_DealerId", "dbo.Dealers");
            DropForeignKey("dbo.Plans", "CategoryID", "dbo.Categories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.InvetoryChangeHistories", new[] { "Inventory_ID" });
            DropIndex("dbo.SalesTransactions", new[] { "InventoryId" });
            DropIndex("dbo.SalesTransactions", new[] { "DealerId" });
            DropIndex("dbo.SalesTransactions", new[] { "UserId" });
            DropIndex("dbo.SalesTransactions", new[] { "PlanId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Inventories", new[] { "StockOutBy_Id" });
            DropIndex("dbo.Inventories", new[] { "StockInBy_Id" });
            DropIndex("dbo.Inventories", new[] { "Product_ID" });
            DropIndex("dbo.Inventories", new[] { "Dealer_DealerId" });
            DropIndex("dbo.Inventories", new[] { "IMEI" });
            DropIndex("dbo.Plans", new[] { "CategoryID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.InvetoryChangeHistories");
            DropTable("dbo.SalesTransactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Products");
            DropTable("dbo.Inventories");
            DropTable("dbo.Dealers");
            DropTable("dbo.Plans");
            DropTable("dbo.Categories");
        }
    }
}
