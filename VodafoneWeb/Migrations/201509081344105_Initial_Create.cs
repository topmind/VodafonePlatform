namespace VodafoneWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Create : DbMigration
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
                        InventoryId = c.Int(nullable: false, identity: true),
                        DealerId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        IMEI = c.String(maxLength: 450),
                        StockInById = c.String(maxLength: 128),
                        StockInDate = c.DateTime(nullable: false),
                        StockOutById = c.String(maxLength: 128),
                        StockOutDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Type = c.Int(),
                        PurchasedFrom = c.String(),
                        PurchasedById = c.String(maxLength: 128),
                        DefferCode = c.String(),
                        DefferName = c.String(),
                        DefferOrderNo = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.InventoryId)
                .ForeignKey("dbo.Dealers", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.PurchasedById)
                .ForeignKey("dbo.AspNetUsers", t => t.StockInById)
                .ForeignKey("dbo.AspNetUsers", t => t.StockOutById)
                .Index(t => t.DealerId)
                .Index(t => t.ProductId)
                .Index(t => t.StockInById)
                .Index(t => t.StockOutById)
                .Index(t => t.PurchasedById);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ProductCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        ProductCategoryId = c.Int(nullable: false, identity: true),
                        ProductCategoryName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductCategoryId);
            
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
                        InventoryId = c.Int(),
                        ProductId = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        RefferA = c.String(),
                        RefferB = c.String(),
                        Gift = c.String(),
                        Audit = c.Int(),
                        Note = c.String(),
                        IsChanged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dealers", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.Inventories", t => t.InventoryId)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PlanId)
                .Index(t => t.UserId)
                .Index(t => t.DealerId)
                .Index(t => t.InventoryId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.InvetoryChangeHistories",
                c => new
                    {
                        InvetoryChangeHistoryId = c.Int(nullable: false, identity: true),
                        IMEI = c.String(maxLength: 450),
                        ChangeDate = c.DateTime(nullable: false),
                        OldOperationType = c.Int(nullable: false),
                        NewOperationType = c.Int(nullable: false),
                        OperatedByEmployeeID = c.String(),
                        SalesId = c.Int(),
                        FromDealer = c.String(),
                        ToDealer = c.String(),
                    })
                .PrimaryKey(t => t.InvetoryChangeHistoryId)
                .ForeignKey("dbo.SalesTransactions", t => t.SalesId)
                .Index(t => t.IMEI)
                .Index(t => t.SalesId);
            
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
            DropForeignKey("dbo.InvetoryChangeHistories", "SalesId", "dbo.SalesTransactions");
            DropForeignKey("dbo.SalesTransactions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesTransactions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.SalesTransactions", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.SalesTransactions", "InventoryId", "dbo.Inventories");
            DropForeignKey("dbo.SalesTransactions", "DealerId", "dbo.Dealers");
            DropForeignKey("dbo.Inventories", "StockOutById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inventories", "StockInById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inventories", "PurchasedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Inventories", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.Inventories", "DealerId", "dbo.Dealers");
            DropForeignKey("dbo.Plans", "CategoryID", "dbo.Categories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.InvetoryChangeHistories", new[] { "SalesId" });
            DropIndex("dbo.InvetoryChangeHistories", new[] { "IMEI" });
            DropIndex("dbo.SalesTransactions", new[] { "ProductId" });
            DropIndex("dbo.SalesTransactions", new[] { "InventoryId" });
            DropIndex("dbo.SalesTransactions", new[] { "DealerId" });
            DropIndex("dbo.SalesTransactions", new[] { "UserId" });
            DropIndex("dbo.SalesTransactions", new[] { "PlanId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.Inventories", new[] { "PurchasedById" });
            DropIndex("dbo.Inventories", new[] { "StockOutById" });
            DropIndex("dbo.Inventories", new[] { "StockInById" });
            DropIndex("dbo.Inventories", new[] { "ProductId" });
            DropIndex("dbo.Inventories", new[] { "DealerId" });
            DropIndex("dbo.Plans", new[] { "CategoryID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.InvetoryChangeHistories");
            DropTable("dbo.SalesTransactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Inventories");
            DropTable("dbo.Dealers");
            DropTable("dbo.Plans");
            DropTable("dbo.Categories");
        }
    }
}
