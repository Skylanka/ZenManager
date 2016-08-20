namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoadAll : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttributesTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Values = c.String(),
                        DefaultValue = c.String(),
                        IsDefault = c.Boolean(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectsRequirenmentAttributeTypes",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        RequirenmentTypeId = c.Int(nullable: false),
                        AttributeTypeId = c.Int(nullable: false),
                        RequirementsDescription_Id = c.Int(),
                        AttributeDescription_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.RequirenmentTypeId, t.AttributeTypeId })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.RequirementsTypes", t => t.RequirementsDescription_Id)
                .ForeignKey("dbo.AttributesTypes", t => t.AttributeDescription_Id)
                .Index(t => t.ProjectId)
                .Index(t => t.RequirementsDescription_Id)
                .Index(t => t.AttributeDescription_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Hometown = c.String(),
                        Phone = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        Avatar = c.String(),
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
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueTag = c.String(),
                        Type = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Author_Id = c.Int(),
                        RequirementBO_Id = c.Int(),
                        TypeDescription_Id = c.Int(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_Id)
                .ForeignKey("dbo.Requirements", t => t.RequirementBO_Id)
                .ForeignKey("dbo.RequirementsTypes", t => t.TypeDescription_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.RequirementBO_Id)
                .Index(t => t.TypeDescription_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Type_Id = c.Int(nullable: false),
                        RequirementBO_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttributesTypes", t => t.Type_Id, cascadeDelete: true)
                .ForeignKey("dbo.Requirements", t => t.RequirementBO_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.RequirementBO_Id);
            
            CreateTable(
                "dbo.RequirementsTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProjectBOApplicationUsers",
                c => new
                    {
                        ProjectBO_Id = c.Int(nullable: false),
                        ApplicationUser_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectBO_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.Projects", t => t.ProjectBO_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ApplicationUser_Id, cascadeDelete: false)
                .Index(t => t.ProjectBO_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "AttributeDescription_Id", "dbo.AttributesTypes");
            DropForeignKey("dbo.ProjectBOApplicationUsers", "ApplicationUser_Id", "dbo.Users");
            DropForeignKey("dbo.ProjectBOApplicationUsers", "ProjectBO_Id", "dbo.Projects");
            DropForeignKey("dbo.Requirements", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Requirements", "TypeDescription_Id", "dbo.RequirementsTypes");
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "RequirementsDescription_Id", "dbo.RequirementsTypes");
            DropForeignKey("dbo.Requirements", "RequirementBO_Id", "dbo.Requirements");
            DropForeignKey("dbo.Requirements", "Author_Id", "dbo.Users");
            DropForeignKey("dbo.Attributes", "RequirementBO_Id", "dbo.Requirements");
            DropForeignKey("dbo.Attributes", "Type_Id", "dbo.AttributesTypes");
            DropForeignKey("dbo.ProjectsRequirenmentAttributeTypes", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Author_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropIndex("dbo.ProjectBOApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProjectBOApplicationUsers", new[] { "ProjectBO_Id" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Attributes", new[] { "RequirementBO_Id" });
            DropIndex("dbo.Attributes", new[] { "Type_Id" });
            DropIndex("dbo.Requirements", new[] { "Project_Id" });
            DropIndex("dbo.Requirements", new[] { "TypeDescription_Id" });
            DropIndex("dbo.Requirements", new[] { "RequirementBO_Id" });
            DropIndex("dbo.Requirements", new[] { "Author_Id" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Projects", new[] { "Author_Id" });
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "AttributeDescription_Id" });
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "RequirementsDescription_Id" });
            DropIndex("dbo.ProjectsRequirenmentAttributeTypes", new[] { "ProjectId" });
            DropTable("dbo.ProjectBOApplicationUsers");
            DropTable("dbo.Roles");
            DropTable("dbo.RequirementsTypes");
            DropTable("dbo.Attributes");
            DropTable("dbo.Requirements");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectsRequirenmentAttributeTypes");
            DropTable("dbo.AttributesTypes");
        }
    }
}
