using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.BusinessObjects;

namespace Model.Base
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hometown { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Avatar { get; set; }
        public ICollection<ProjectBO> Projects { get; set; } 
        public ICollection<InvitationBO> Invitations { get; set; } 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class CopyOfApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hometown { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Avatar { get; set; }
        public ICollection<ProjectBO> Projects { get; set; }
        public ICollection<InvitationBO> Invitations { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CopyOfApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    public class UserLogin : IdentityUserLogin<int>
    {
    }

    public class CopyOfUserLogin : IdentityUserLogin<int>
    {
    }

    public class CopyOfRole : IdentityRole<int, UserRole>
    {
        public string Description { get; set; }  // Custom description field on roles
    }

    public class CopyOfCopyOfUserLogin : IdentityUserLogin<int>
    {
    }

    public class Role : IdentityRole<int, UserRole>
    {
        public string Description { get; set; }  // Custom description field on roles
    }

    public class CopyOfCopyOfUserRole : IdentityUserRole<int>
    {
    }

    public class CopyOfUserRole : IdentityUserRole<int>
    {
    }

    public class UserRole : IdentityUserRole<int>
    {
    }

    public class CopyOfCustomUserStore : UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public CopyOfCustomUserStore(ApplicationDbContext context)
        : base(context)
        {
        }
    }

    public class UserClaim : IdentityUserClaim<int>
    {
    }

    public class CopyOfApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public CopyOfApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<ProjectBO> Projects { get; set; }
        public DbSet<RequirementBO> Requirements { get; set; }
        public DbSet<AttributeBO> Attributes { get; set; }
        public DbSet<RequirementsDescriptionBO> RequirementsDescription { get; set; }
        public DbSet<AttributeDescriptionBO> AttributeDescription { get; set; }
        public DbSet<ProjectsRequirenmentAttributeTypesBO> ProjectsRequirenmentAttributeTypes { get; set; }
        public DbSet<CommentBO> Comments { get; set; }
        public DbSet<IssueBO> Issues { get; set; }
        public DbSet<RequirementsLinkBO> ReqLinks { get; set; }
        public DbSet<ChatRecordBO> ChatRecords { get; set; }
        public DbSet<DocumentationBO> Documentations { get; set; }
        public DbSet<InvitationBO> Invitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // This needs to go before the other rules!
            base.OnModelCreating(modelBuilder);

            BOToTableMapping(modelBuilder);
            ReferencesMapping(modelBuilder);
            IgnoreMapping(modelBuilder);

        }

        private static void IgnoreMapping(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Ignore<DiscretEnvironmentParameterBO>();
        }

        private static void ReferencesMapping(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Users).WithMany(u => u.Projects);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Requirements);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Docs);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.ProjectsRequirenmentAttributeTypes);
            modelBuilder.Entity<ProjectBO>().HasRequired(p => p.CreatedBy);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Invitations);
            modelBuilder.Entity<RequirementBO>().HasMany(r => r.Attributes);
            modelBuilder.Entity<AttributeBO>().HasRequired(a => a.Type);
            modelBuilder.Entity<AttributeDescriptionBO>().HasMany(a => a.ProjectsRequirenmentAttributeTypes);
            modelBuilder.Entity<RequirementsDescriptionBO>().HasMany(r => r.ProjectsRequirenmentAttributeTypes);
            modelBuilder.Entity<IssueBO>().HasMany(i => i.Comments);
            modelBuilder.Entity<IssueBO>().HasMany(i => i.Requirements).WithMany();
        }

        private static void BOToTableMapping(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<AttributeBO>().ToTable("Attributes");
            modelBuilder.Entity<AttributeDescriptionBO>().ToTable("AttributesTypes");
            modelBuilder.Entity<ProjectBO>().ToTable("Projects");
            modelBuilder.Entity<RequirementBO>().ToTable("Requirements");
            modelBuilder.Entity<RequirementsDescriptionBO>().ToTable("RequirementsTypes");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<ProjectsRequirenmentAttributeTypesBO>().ToTable("ProjectsRequirenmentAttributeTypes");
            modelBuilder.Entity<CommentBO>().ToTable("Comments");
            modelBuilder.Entity<IssueBO>().ToTable("Issues");
            modelBuilder.Entity<RequirementsLinkBO>().ToTable("ReqLinks");
            modelBuilder.Entity<ChatRecordBO>().ToTable("ChatRecords");
            modelBuilder.Entity<DocumentationBO>().ToTable("Documentation");
            modelBuilder.Entity<InvitationBO>().ToTable("Invitations");
        }

        public static CopyOfApplicationDbContext Create()
        {
            return new CopyOfApplicationDbContext();
        }
    }

    public class CustomUserStore : UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
        : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<Role, int, UserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<ProjectBO> Projects { get; set; }
        public DbSet<RequirementBO> Requirements { get; set; }
        public DbSet<AttributeBO> Attributes { get; set; }
        public DbSet<RequirementsDescriptionBO> RequirementsDescription { get; set; }
        public DbSet<AttributeDescriptionBO> AttributeDescription { get; set; } 
        public DbSet<ProjectsRequirenmentAttributeTypesBO> ProjectsRequirenmentAttributeTypes { get; set; }
        public DbSet<CommentBO> Comments { get; set; }
        public DbSet<IssueBO> Issues { get; set; }  
        public DbSet<RequirementsLinkBO> ReqLinks { get; set; } 
        public DbSet<ChatRecordBO> ChatRecords { get; set; }
        public DbSet<DocumentationBO> Documentations { get; set; }
        public DbSet<InvitationBO> Invitations { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // This needs to go before the other rules!
            base.OnModelCreating(modelBuilder);

            BOToTableMapping(modelBuilder);
            ReferencesMapping(modelBuilder);
            IgnoreMapping(modelBuilder);

        }

        private static void IgnoreMapping(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Ignore<DiscretEnvironmentParameterBO>();
        }

        private static void ReferencesMapping(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Users).WithMany(u => u.Projects);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Requirements);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Docs);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.ProjectsRequirenmentAttributeTypes);
            modelBuilder.Entity<ProjectBO>().HasRequired(p => p.CreatedBy);
            modelBuilder.Entity<ProjectBO>().HasMany(p => p.Invitations);
            modelBuilder.Entity<RequirementBO>().HasMany(r => r.Attributes);
            modelBuilder.Entity<AttributeBO>().HasRequired(a => a.Type);
            modelBuilder.Entity<AttributeDescriptionBO>().HasMany(a => a.ProjectsRequirenmentAttributeTypes);
            modelBuilder.Entity<RequirementsDescriptionBO>().HasMany(r => r.ProjectsRequirenmentAttributeTypes);
            modelBuilder.Entity<IssueBO>().HasMany(i => i.Comments);
            modelBuilder.Entity<IssueBO>().HasMany(i => i.Requirements).WithMany();
        }

        private static void BOToTableMapping(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<AttributeBO>().ToTable("Attributes");
            modelBuilder.Entity<AttributeDescriptionBO>().ToTable("AttributesTypes");
            modelBuilder.Entity<ProjectBO>().ToTable("Projects");
            modelBuilder.Entity<RequirementBO>().ToTable("Requirements");
            modelBuilder.Entity<RequirementsDescriptionBO>().ToTable("RequirementsTypes");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<ProjectsRequirenmentAttributeTypesBO>().ToTable("ProjectsRequirenmentAttributeTypes");
            modelBuilder.Entity<CommentBO>().ToTable("Comments");
            modelBuilder.Entity<IssueBO>().ToTable("Issues");
            modelBuilder.Entity<RequirementsLinkBO>().ToTable("ReqLinks");
            modelBuilder.Entity<ChatRecordBO>().ToTable("ChatRecords");
            modelBuilder.Entity<DocumentationBO>().ToTable("Documentation");
            modelBuilder.Entity<InvitationBO>().ToTable("Invitations");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}