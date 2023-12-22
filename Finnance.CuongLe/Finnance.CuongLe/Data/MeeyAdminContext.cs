using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Finnance.CuongLe.Data.Entities.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Meey.Admin.Data
{
    public partial class MeeyAdminContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, IdentityUserLogin<int>, RoleClaim, IdentityUserToken<int>>
    {
        public MeeyAdminContext()
        {
        }

        public MeeyAdminContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.Locked);
                entity.HasIndex(e => e.UserName);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.HasIndex(e => e.FullName);
                entity.HasIndex(e => e.PhoneNumber);
                entity.HasIndex(e => e.ExtPhoneNumber);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.OTPSecretKey).HasMaxLength(250);
                entity.Property(e => e.OTPEncodedKey).HasMaxLength(250);
                entity.Property(e => e.OTPQrCodeImage).HasMaxLength(4000);
                entity.Property(e => e.UserName).HasMaxLength(50);
                entity.Property(e => e.FullName).HasMaxLength(80);
                entity.Property(e => e.Address).HasMaxLength(550);
                entity.Property(e => e.VerifyCode).HasMaxLength(15);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.ReasonLock).HasMaxLength(1000);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Users_PositionId");
                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Users_CountryId");
                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Users_DepartmentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Roles_OrganizationId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("teams");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Code).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Teams_OrganizationId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.OtherName).HasMaxLength(500);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasIndex(c => c.RoleId);
                entity.HasIndex(c => c.UserId);
                entity.ToTable("userroles");
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRole_UserId");
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRole_RoleId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.TeamId);
                entity.ToTable("userteams");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTeams)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserTeams_UserId");
                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UserTeams)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_UserTeams_TeamId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable("positions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Code).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable("roleclaims");
            });
            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.ToTable("userclaims");
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Action);
                entity.HasIndex(e => e.Controller);
                entity.Property(e => e.Title).HasMaxLength(250);
                entity.Property(e => e.Group).HasMaxLength(250);
                entity.Property(e => e.Types).HasMaxLength(250);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Controller).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("FK_Permissions_OrganizationId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("departments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserProduct>(entity =>
            {
                entity.ToTable("userproducts");
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.ProductId);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProducts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserProducts_UserId");
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.UserProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_UserProducts_ProductId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<LogActivity>(entity =>
            {
                entity.ToTable("logactivities");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Url);
                entity.HasIndex(e => e.ObjectId);
                entity.HasIndex(e => e.Controller);
                entity.Property(e => e.Ip).HasMaxLength(20);
                entity.Property(e => e.Url).HasMaxLength(250);
                entity.Property(e => e.Method).HasMaxLength(20);
                entity.Property(e => e.Action).HasMaxLength(150);
                entity.Property(e => e.Notes).HasMaxLength(2000);
                entity.Property(e => e.ObjectId).HasMaxLength(500);
                entity.Property(e => e.Controller).HasMaxLength(150);
                entity.Property(e => e.Ip).HasColumnType("longtext");
                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogActivities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_LogActivities_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<PrefixNumber>(entity =>
            {
                entity.ToTable("prefixnumbers");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Phone).IsUnique();
                entity.HasIndex(e => e.Prefix).IsUnique();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organizations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Leader).HasMaxLength(150);
                entity.Property(e => e.LeaderPhone).HasMaxLength(20);
                entity.Property(e => e.LeaderEmail).HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Website).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserActivity>(entity =>
            {
                entity.ToTable("useractivities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ip).HasMaxLength(50);
                entity.Property(e => e.Country).HasMaxLength(150);
                entity.Property(e => e.Os).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Browser).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserActivity_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<LogException>(entity =>
            {
                entity.ToTable("logexceptions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Exception).HasMaxLength(4000);
                entity.Property(e => e.StackTrace).HasMaxLength(4000);
                entity.Property(e => e.InnerException).HasMaxLength(4000);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogExceptions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_LogExceptions_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserDistrict>(entity =>
            {
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.DistrictId);
                entity.ToTable("userdistricts");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserDistricts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserDistricts_UserId");
                entity.HasOne(d => d.District)
                    .WithMany(p => p.UserDistricts)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_UserDistricts_DistrictId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<RequestFilter>(entity =>
            {
                entity.ToTable("requestfilters");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Controller);
                entity.Property(e => e.FilterData).IsRequired();
                entity.Property(e => e.Controller).HasMaxLength(150);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Controller).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.RequestFilters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RequestFilters_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("userpermissions");
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.PermissionId);
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserPermissions_UserId");
                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_UserPermissions_PermissionId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("rolepermissions");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RolePermissions_RoleId");
                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_RolePermissions_PermissionId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<LinkPermission>(entity =>
            {
                entity.ToTable("linkpermissions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Order);
                entity.Property(e => e.Group).HasMaxLength(50);
                entity.Property(e => e.CssIcon).HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Link).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.LinkPermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_LinkPermissions_PermissionId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.Property(e => e.ProviderKey).HasMaxLength(250);
                entity.Property(e => e.LoginProvider).HasMaxLength(250);
            });
            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.LoginProvider).HasMaxLength(250);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("cities");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.EnglishName);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.EnglishName).HasMaxLength(150);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_City_CountryId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Ward>(entity =>
            {
                entity.ToTable("wards");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.EnglishName);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_Ward_DistrictId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Street>(entity =>
            {
                entity.ToTable("streets");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.EnglishName);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.City)
                    .WithMany(p => p.Streets)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Street_CityId");
                entity.HasOne(d => d.District)
                    .WithMany(p => p.Streets)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_Street_DistrictId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countries");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.SystemName);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.DialingCode).HasMaxLength(10);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.SystemName).IsRequired().HasMaxLength(50);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<District>(entity =>
            {
                entity.ToTable("districts");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Title);
                entity.HasIndex(e => e.EnglishName);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.EnglishName).HasMaxLength(150);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
                entity.HasOne(d => d.City)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_District_CityId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<SmsTemplate>(entity =>
            {
                entity.ToTable("smstemplates");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(550);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<EmailTemplate>(entity =>
            {
                entity.ToTable("emailtemplates");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(550);
                entity.HasOne(d => d.SmtpAccount)
                    .WithMany(p => p.EmailTemplates)
                    .HasForeignKey(d => d.SmtpAccountId)
                    .HasConstraintName("FK_EmailTemplates_SmtpAccountId");
                entity.HasOne(d => d.EmailTemplateWrapper)
                    .WithMany(p => p.EmailTemplates)
                    .HasForeignKey(d => d.EmailTemplateWrapperId)
                    .HasConstraintName("FK_EmailTemplates_EmailTemplateWrapperId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<EmailTemplateWrapper>(entity =>
            {
                entity.ToTable("emailtemplatewrappers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            modelBuilder.Entity<Node>(entity =>
            {
                entity.ToTable("nodes");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Left);
                entity.HasIndex(e => e.Right);
                entity.HasIndex(e => e.RootId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Branch);
                entity.HasIndex(e => e.ParentId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.HasOne(d => d.Root)
                    .WithMany(p => p.Descendants)
                    .HasForeignKey(d => d.RootId)
                    .HasConstraintName("FK_Descendants_RootId");
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Childrens)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Children_ParentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<NodeV1>(entity =>
            {
                entity.ToTable("maf_nodes_v1");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.Phone);
                entity.HasIndex(e => e.MeeyId);
                entity.Property(e => e.Code).HasMaxLength(250);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Phone).HasMaxLength(250);
                entity.Property(e => e.Avatar).HasMaxLength(250);
                entity.Property(e => e.MeeyId).HasMaxLength(250);
                entity.Property(e => e.JoinDate).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<NodeBinary>(entity =>
            {
                entity.ToTable("nodebinaries");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Left);
                entity.HasIndex(e => e.Right);
                entity.HasIndex(e => e.RootId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Branch);
                entity.HasIndex(e => e.ParentId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.HasOne(d => d.Root)
                    .WithMany(p => p.Descendants)
                    .HasForeignKey(d => d.RootId)
                    .HasConstraintName("FK_NodeBinary_Descendants_RootId");
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Childrens)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_NodeBinary_Children_ParentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("banks");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.Property(e => e.Icon).HasMaxLength(250);
                entity.Property(e => e.BankCode).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(550);
                entity.Property(e => e.AccountNumber).HasMaxLength(50);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Notify>(entity =>
            {
                entity.ToTable("notifies");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IsRead);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.Property(e => e.Content).HasMaxLength(550);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.User)
                   .WithMany(p => p.Notifies)
                   .HasForeignKey(d => d.UserId)
                   .HasConstraintName("FK_Notify_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<EndPoint>(entity =>
            {
                entity.ToTable("endpoints");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.HasIndex(e => e.SystemName);
                entity.Property(e => e.Token).HasMaxLength(250);
                entity.Property(e => e.AppKey).HasMaxLength(100);
                entity.Property(e => e.XApiKey).HasMaxLength(100);
                entity.Property(e => e.SecretKey).HasMaxLength(100);
                entity.Property(e => e.XClientId).HasMaxLength(100);
                entity.Property(e => e.Url).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Domain).IsRequired().HasMaxLength(150);
                entity.Property(e => e.SystemName).IsRequired().HasMaxLength(50);
                entity.HasOne(d => d.DomainItem)
                  .WithMany(p => p.EndPoints)
                  .HasForeignKey(d => d.DomainId)
                  .HasConstraintName("FK_EndPoints_DomainId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Domain>(entity =>
            {
                entity.ToTable("domains");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Value);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.Property(e => e.Token).HasMaxLength(250);
                entity.Property(e => e.AppKey).HasMaxLength(250);
                entity.Property(e => e.XApiKey).HasMaxLength(250);
                entity.Property(e => e.SecretKey).HasMaxLength(250);
                entity.Property(e => e.XClientId).HasMaxLength(250);
                entity.Property(e => e.Label).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Value).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Group).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Facility>(entity =>
            {
                entity.ToTable("facilities");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.Property(e => e.Category).HasMaxLength(250);
                entity.Property(e => e.Address).HasMaxLength(1000);
                entity.Property(e => e.ImageUrl).HasMaxLength(1000);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(1000);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<ForgotCode>(entity =>
            {
                entity.ToTable("forgotcodes");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Used);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.DialingCode).HasMaxLength(10);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.VertifyCode).IsRequired().HasMaxLength(6);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<SmtpAccount>(entity =>
            {
                entity.ToTable("smtpaccounts");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Type);
                entity.Property(e => e.Host).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.EmailFrom).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<WebsiteConfig>(entity =>
            {
                entity.ToTable("websiteconfigs");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsDelete);
                entity.HasIndex(e => e.Domain).IsUnique();
                entity.Property(e => e.Domain).HasMaxLength(50);
                entity.Property(e => e.ContactName).HasMaxLength(150);
                entity.Property(e => e.ContactPhone).HasMaxLength(50);
                entity.Property(e => e.ContactEmail).HasMaxLength(150);
                entity.Property(e => e.CustomerPhone).HasMaxLength(50);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // Chat
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.User)
                  .WithMany(p => p.Groups)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Groups_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Files).HasMaxLength(2000);
                entity.Property(e => e.Content).HasMaxLength(2000);
                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_Messages_TeamId");
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Messages_GroupId");
                entity.HasOne(d => d.Send)
                    .WithMany(p => p.SendMessages)
                    .HasForeignKey(d => d.SendId)
                    .HasConstraintName("FK_SendMessages_SendId");
                entity.HasOne(d => d.Receive)
                    .WithMany(p => p.ReceiveMessages)
                    .HasForeignKey(d => d.ReceiveId)
                    .HasConstraintName("FK_ReceiveMessages_ReceiveId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("usergroups");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                  .WithMany(p => p.UserGroups)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_UserGroups_UserId");
                entity.HasOne(d => d.Group)
                  .WithMany(p => p.UserGroups)
                  .HasForeignKey(d => d.GroupId)
                  .HasConstraintName("FK_UserGroups_GroupId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.ToTable("configurations");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.VfoneApi).HasMaxLength(150);
                entity.Property(e => e.VfoneApiKeyAccess).HasMaxLength(150);
                entity.Property(e => e.ZaloMiniPartnerKey).HasMaxLength(150);
                entity.Property(e => e.ZaloMiniSaleEmail).HasMaxLength(150);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("tickets");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Title);
                entity.Property(e => e.Attachments).HasMaxLength(2000);
                entity.Property(e => e.InternalNote).HasMaxLength(2000);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
                entity.HasOne(d => d.User)
                  .WithMany(p => p.Tickets)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_Tickets_UserId");
                entity.HasOne(d => d.AssignTo)
                   .WithMany(p => p.TicketAssigns)
                   .HasForeignKey(d => d.AssignToId)
                   .HasConstraintName("FK_TicketAssigns_AssignToId");
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Tickets_CategoryId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<TicketDetail>(entity =>
            {
                entity.ToTable("ticketdetails");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quote).HasMaxLength(2000);
                entity.Property(e => e.Content).HasMaxLength(2000);
                entity.Property(e => e.Attachments).HasMaxLength(2000);
                entity.Property(e => e.InternalNote).HasMaxLength(2000);
                entity.Property(e => e.QuoteAttachments).HasMaxLength(2000);
                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketDetails)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK_TicketDetails_TicketId");
                entity.HasOne(d => d.AnswerBy)
                   .WithMany(p => p.TicketDetailAnswers)
                   .HasForeignKey(d => d.AnswerById)
                   .HasConstraintName("FK_TicketDetailAnswers_AnswerById");
                entity.HasOne(d => d.AssignTo)
                   .WithMany(p => p.TicketDetailAssigns)
                   .HasForeignKey(d => d.AssignToId)
                   .HasConstraintName("FK_TicketDetailAssigns_AssignToId");
                entity.HasOne(d => d.QuestionBy)
                   .WithMany(p => p.TicketDetailQuestions)
                   .HasForeignKey(d => d.QuestionById)
                   .HasConstraintName("FK_TicketDetailQuestions_QuestionById");
                entity.HasOne(d => d.QuoteAnswerBy)
                   .WithMany(p => p.TicketDetailQuoteAnswers)
                   .HasForeignKey(d => d.QuoteAnswerById)
                   .HasConstraintName("FK_TicketDetailQuoteAnswers_QuoteAnswerById");
                entity.HasOne(d => d.QuoteQuestionBy)
                   .WithMany(p => p.TicketDetailQuoteQuestions)
                   .HasForeignKey(d => d.QuoteQuestionById)
                   .HasConstraintName("FK_TicketDetailQuoteQuestions_QuoteQuestionById");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<TicketCategory>(entity =>
            {
                entity.ToTable("ticketcategories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.SvgIcon).HasMaxLength(2000);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // File Manager
            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable("files");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Link).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Extension).IsRequired().HasMaxLength(20);
                entity.HasOne(d => d.Folder)
                  .WithMany(p => p.Files)
                  .HasForeignKey(d => d.FolderId)
                  .HasConstraintName("FK_Files_FolderId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("folders");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.Parent)
                  .WithMany(p => p.Folders)
                  .HasForeignKey(d => d.ParentId)
                  .HasConstraintName("FK_Folders_ParentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // MeeyId
            modelBuilder.Entity<MLUser>(entity =>
            {
                entity.ToTable("ml_user");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Phone).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.UserName).HasMaxLength(250);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.MLSaleUsers)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_MLSaleUsers_SaleId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.MLSupportUsers)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_MLSupportUsers_SupportId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLUserAssign>(entity =>
            {
                entity.ToTable("ml_user_assign");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.MLUser)
                    .WithMany(p => p.MLUserAssigns)
                    .HasForeignKey(d => d.MLUserId)
                    .HasConstraintName("FK_MLUserAssigns_MLUserId");
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ToMLUserAssigns)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_ToMLUserAssigns_ToUserId");
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.FromMLUserAssigns)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK_FromMLUserAssigns_FromUserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // MeeyArticle
            modelBuilder.Entity<MLArticle>(entity =>
            {
                entity.ToTable("ml_article");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.SupportId);
                entity.HasIndex(c => c.CreatedDate);
                entity.HasIndex(c => c.UpdatedDate);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.MLUser)
                    .WithMany(p => p.MLArticles)
                    .HasForeignKey(d => d.MLUserId)
                    .HasConstraintName("FK_MLArticles_MLUserId");
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.MLSaleArticles)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_MLSaleArticles_SaleId");
                entity.HasOne(d => d.Poster)
                    .WithMany(p => p.MLPosterArticles)
                    .HasForeignKey(d => d.PosterId)
                    .HasConstraintName("FK_MLPosterArticles_PosterId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.MLSupportArticles)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_MLSupportArticles_SupportId");
                entity.HasOne(d => d.Approve)
                    .WithMany(p => p.MLApproveArticles)
                    .HasForeignKey(d => d.ApproveId)
                    .HasConstraintName("FK_MLApproveArticles_ApproveId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLArticleReport>(entity =>
            {
                entity.ToTable("ml_article_report");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.HasOne(d => d.MLArticle)
                    .WithMany(p => p.MLArticleReports)
                    .HasForeignKey(d => d.MLArticleId)
                    .HasConstraintName("FK_MLArticleReports_MLArticleId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLArticleAssign>(entity =>
            {
                entity.ToTable("ml_article_assign");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.MLArticle)
                    .WithMany(p => p.MLArticleAssigns)
                    .HasForeignKey(d => d.MLArticleId)
                    .HasConstraintName("FK_MLArticleAssigns_MLArticleId");
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ToMLArticleAssigns)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_ToMLArticleAssigns_ToUserId");
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.FromMLArticleAssigns)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK_FromMLArticleAssigns_FromUserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            modelBuilder.Entity<MLPartner>(entity =>
            {
                entity.ToTable("ml_partner");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PartnerKey).HasMaxLength(100);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLCompany>(entity =>
            {
                entity.ToTable("ml_company");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.CreatorId).HasMaxLength(50);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MLCompanies)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MLCompanies_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            modelBuilder.Entity<MLSchedule>(entity =>
            {
                entity.ToTable("ml_schedule");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.ScheduleTimes).HasMaxLength(4000);
                entity.Property(e => e.ScheduleDates).HasMaxLength(4000);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MLSchedules)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MLSchedules_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLEmployee>(entity =>
            {
                entity.ToTable("ml_employees");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.Status).HasMaxLength(100);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MeeyUserId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.MLCompany)
                    .WithMany(p => p.MLEmployees)
                    .HasForeignKey(d => d.MLCompanyId)
                    .HasConstraintName("FK_MLEmployees_MLCompanyId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLCouponHMC>(entity =>
            {
                entity.ToTable("ml_coupon_hmc");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MLCouponHMCs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MLCouponHMCs_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLPartnerCode>(entity =>
            {
                entity.ToTable("ml_partner_code");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.PartnerKey).IsUnique();
                entity.HasOne(d => d.MLPartner)
                    .WithMany(p => p.MLPartnerCodes)
                    .HasForeignKey(d => d.MLPartnerId)
                    .HasConstraintName("FK_MLPartnerCodes_MLPartnerId");
                entity.Property(e => e.PartnerKey).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLScheduleLog>(entity =>
            {
                entity.ToTable("ml_schedule_logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Ip).HasMaxLength(50);
                entity.Property(e => e.Action).IsRequired();
                entity.Property(e => e.User).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
                entity.HasOne(d => d.MLSchedule)
                    .WithMany(p => p.MLScheduleLogs)
                    .HasForeignKey(d => d.MLScheduleId)
                    .HasConstraintName("FK_MLScheduleLogs_MLScheduleId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLEmployeeHistory>(entity =>
            {
                entity.ToTable("ml_employee_histories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Executor).HasMaxLength(250);
                entity.Property(e => e.PrevStatus).HasMaxLength(100);
                entity.Property(e => e.CurentStatus).HasMaxLength(100);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MLEmployeeHistories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MLEmployeeHistories_UserId");
                entity.HasOne(d => d.MLEmployee)
                    .WithMany(p => p.MLEmployeeHistories)
                    .HasForeignKey(d => d.MLEmployeeId)
                    .HasConstraintName("FK_MLEmployeeHistories_MLEmployeeId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MLScheduleHistory>(entity =>
            {
                entity.ToTable("ml_schedule_histories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MLScheduleHistories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MLScheduleHistories_UserId");
                entity.HasOne(d => d.MLSchedule)
                    .WithMany(p => p.MLScheduleHistories)
                    .HasForeignKey(d => d.MLScheduleId)
                    .HasConstraintName("FK_MLScheduleHistories_MLScheduleId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // MeeyPay
            modelBuilder.Entity<MPInvoice>(entity =>
            {
                entity.ToTable("mp_invoice");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ParentId);
                entity.HasIndex(e => e.CreatedDate);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.TransactionCode).HasMaxLength(50);
                entity.Property(e => e.RevenueCode).HasMaxLength(50);
                entity.Property(e => e.PaymentMethod).HasMaxLength(150);
                entity.Property(e => e.TaxCode).HasMaxLength(50);
                entity.Property(e => e.CustomerName).HasMaxLength(250);
                entity.Property(e => e.CustomerEmail).HasMaxLength(250);
                entity.Property(e => e.CustomerPhone).HasMaxLength(250);
                entity.Property(e => e.CustomerAddress).HasMaxLength(250);
                entity.Property(e => e.BankName).HasMaxLength(250);
                entity.Property(e => e.Content).HasMaxLength(500);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).HasMaxLength(250);
                entity.Property(e => e.UpdatedBy).HasMaxLength(250);
                entity.HasOne(d => d.UserInvoice)
                    .WithMany(p => p.MPTransactionInvoices)
                    .HasForeignKey(d => d.UserInvoiceId)
                    .HasConstraintName("FK_MPTransactionInvoices_UserId");
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Childrens)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Invoice_Children_ParentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MPRevenue>(entity =>
            {
                entity.ToTable("mp_revenues");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Code).IsUnique();
                entity.Property(e => e.Note).HasMaxLength(300);
                entity.Property(e => e.BankNumber).HasMaxLength(50);
                entity.Property(e => e.CustomerName).HasMaxLength(250);
                entity.Property(e => e.TransactionCode).HasMaxLength(100);
                entity.Property(e => e.TransactionNote).HasMaxLength(500);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleMPRevenues)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_SaleMPRevenues_SaleId");
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.MPRevenues)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_MPRevenues_CustomerId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MPTransaction>(entity =>
            {
                entity.ToTable("ml_transaction");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.MeeyId);
                entity.HasIndex(c => c.TransactionId);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UrlFile).HasMaxLength(500);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MLTransactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MLTransactions_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MPTransactionAds>(entity =>
            {
                entity.ToTable("mp_transaction_ads");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CreatedDate);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Funds).HasMaxLength(250);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.MeeyPayId).HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(250);
                entity.Property(e => e.UpdatedBy).HasMaxLength(250);
                entity.Property(e => e.RevenueCode).HasMaxLength(50);
                entity.Property(e => e.PaymentMethod).HasMaxLength(150);
                entity.Property(e => e.TransactionCode).HasMaxLength(50);
                entity.Property(e => e.TransactionReason).HasMaxLength(500);
                entity.Property(e => e.TransactionContent).HasMaxLength(500);
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.MPTransactionAds)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_MPTransactionAds_CustomerId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // Meeymap
            modelBuilder.Entity<MMNews>(entity =>
            {
                entity.ToTable("mm_news");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Title);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_News_CategoryId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MMRequest>(entity =>
            {
                entity.ToTable("mm_request");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CompanyName);
                entity.HasIndex(e => e.CompanyPhone);
                entity.HasIndex(e => e.CompanyEmail);
                entity.HasIndex(e => e.CompanyRepresent);
                entity.HasIndex(c => c.MeeyId);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.Carer).HasMaxLength(250);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.TypeName).HasMaxLength(50);
                entity.Property(e => e.ActionName).HasMaxLength(50);
                entity.Property(e => e.PacketName).HasMaxLength(250);
                entity.Property(e => e.CompanyName).HasMaxLength(250);
                entity.Property(e => e.CompanyPhone).HasMaxLength(20);
                entity.Property(e => e.CompanyEmail).HasMaxLength(250);
                entity.Property(e => e.CompanyRepresent).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MMQuestion>(entity =>
            {
                entity.ToTable("mm_questions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Title);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Questions_RootId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MMNewsCategory>(entity =>
            {
                entity.ToTable("mm_news_categories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Categories_ParentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MMLookupHistory>(entity =>
            {
                entity.ToTable("mm_lookup_histories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Address);
                entity.HasIndex(c => c.MeeyUserId);
                entity.HasIndex(c => c.CustomerName);
                entity.HasIndex(c => c.CustomerEmail);
                entity.HasIndex(c => c.CustomerPhone);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.Address).HasMaxLength(1000);
                entity.Property(e => e.CustomerName).HasMaxLength(500);
                entity.Property(e => e.CustomerEmail).HasMaxLength(500);
                entity.Property(e => e.CustomerPhone).HasMaxLength(500);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MeeyUserId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleMMLookupHistories)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_SaleMMLookupHistories_SaleId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.SupportMMLookupHistories)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_SupportMMLookupHistories_SupportId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MMLookupHistoryAssign>(entity =>
            {
                entity.ToTable("mm_lookup_histories_assign");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ToMMLookupHistoryAssigns)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_ToMMLookupHistoryAssigns_ToUserId");
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.FromMMLookupHistoryAssigns)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK_FromMMLookupHistoryAssigns_FromUserId");
                entity.HasOne(d => d.MMLookupHistory)
                    .WithMany(p => p.MMLookupHistoryAssigns)
                    .HasForeignKey(d => d.MMLookupHistoryId)
                    .HasConstraintName("FK_MMLookupHistoryAssigns_MMLookupHistoryId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // MeeyGroup
            modelBuilder.Entity<MGPage>(entity =>
            {
                entity.ToTable("mg_pages");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Link);
                entity.Property(e => e.Link).HasMaxLength(250);
                entity.Property(e => e.Name).HasMaxLength(150);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGNews>(entity =>
            {
                entity.ToTable("mg_news");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.TitleVn);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.TagVn).HasMaxLength(250);
                entity.Property(e => e.TagEn).HasMaxLength(250);
                entity.Property(e => e.TitleVn).HasMaxLength(250);
                entity.Property(e => e.TitleEn).HasMaxLength(250);
                entity.Property(e => e.SlugVn).HasMaxLength(250); //v2
                entity.Property(e => e.SlugEn).HasMaxLength(250); //v2
                entity.Property(e => e.SummaryVn).HasMaxLength(1000);
                entity.Property(e => e.SummaryEn).HasMaxLength(1000);
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.News)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_MGNews_CategoryId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGLink>(entity =>
            {
                entity.ToTable("mg_links");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.Link).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.Property(e => e.NameVn).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGBanner>(entity =>
            {
                entity.ToTable("mg_banners");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameEn);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.Button1Vn).HasMaxLength(100);
                entity.Property(e => e.Button1En).HasMaxLength(100);
                entity.Property(e => e.Button2Vn).HasMaxLength(100);
                entity.Property(e => e.Button2En).HasMaxLength(100);
                entity.Property(e => e.LinkButton1).HasMaxLength(250);
                entity.Property(e => e.LinkButton2).HasMaxLength(250);
                entity.Property(e => e.Image).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.Page)
                    .WithMany(p => p.Banners)
                    .HasForeignKey(d => d.PageId)
                    .HasConstraintName("FK_Banners_PageId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGLeader>(entity =>
            {
                entity.ToTable("mg_leaders");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Link).HasMaxLength(250);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.PositionVn).HasMaxLength(100);
                entity.Property(e => e.PositionEn).HasMaxLength(100);
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Leaders)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Leaders_CategoryLeaderId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGQRLeader>(entity =>
            {
                entity.ToTable("mg_qr_leaders");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameEn);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.Code).HasMaxLength(10);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(20);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.Avatar).HasMaxLength(250);
                entity.Property(e => e.Website).HasMaxLength(250);
                entity.Property(e => e.CompanyEn).HasMaxLength(100);
                entity.Property(e => e.PositionEn).HasMaxLength(100);
                entity.Property(e => e.CompanyVn).HasMaxLength(100);
                entity.Property(e => e.PositionVn).HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGOffice>(entity =>
            {
                entity.ToTable("mg_offices");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.TitleVn);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.TitleVn).HasMaxLength(250);
                entity.Property(e => e.TitleEn).HasMaxLength(250);
                entity.Property(e => e.AddressVn).HasMaxLength(500);
                entity.Property(e => e.AddressEn).HasMaxLength(500);
                entity.Property(e => e.LocationVn).HasMaxLength(100);
                entity.Property(e => e.LocationEn).HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGPartner>(entity =>
            {
                entity.ToTable("mg_partners");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.Link).HasMaxLength(250);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.Property(e => e.DescriptionEn).HasMaxLength(500);
                entity.Property(e => e.DescriptionVn).HasMaxLength(500);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGSection>(entity =>
            {
                entity.ToTable("mg_sections");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGProduct>(entity =>
            {
                entity.ToTable("mg_products");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.Icon).HasMaxLength(250);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.Property(e => e.LinkIos).HasMaxLength(250);
                entity.Property(e => e.ImageFull).HasMaxLength(250);
                entity.Property(e => e.LinkAndroid).HasMaxLength(250);
                entity.Property(e => e.LinkWebsite).HasMaxLength(250);
                entity.Property(e => e.DescriptionVn).HasMaxLength(500);
                entity.Property(e => e.DescriptionEn).HasMaxLength(500);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGMessage>(entity =>
            {
                entity.ToTable("mg_messages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGCategory>(entity =>
            {
                entity.ToTable("mg_categories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGEmployee>(entity =>
            {
                entity.ToTable("mg_employees");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.TitleVn).HasMaxLength(250);
                entity.Property(e => e.TitleEn).HasMaxLength(250);
                entity.Property(e => e.ReviewVn).HasMaxLength(500);
                entity.Property(e => e.ReviewEn).HasMaxLength(500);
                entity.Property(e => e.PositionVn).HasMaxLength(100);
                entity.Property(e => e.PositionEn).HasMaxLength(100);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGAnnounced>(entity =>
            {
                entity.ToTable("mg_announced");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.TitleVn);
                entity.Property(e => e.File).HasMaxLength(250);
                entity.Property(e => e.Link).HasMaxLength(250);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.Property(e => e.TitleVn).HasMaxLength(250);
                entity.Property(e => e.TitleEn).HasMaxLength(250);
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Announceds)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Announceds_CategoryAnnouncedId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGInformation>(entity =>
            {
                entity.ToTable("mg_informations");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CompanyNameVn);
                entity.Property(e => e.Logo).HasMaxLength(250);
                entity.Property(e => e.LogoWhite).HasMaxLength(250);
                entity.Property(e => e.AddressVn).HasMaxLength(500);
                entity.Property(e => e.AddressEn).HasMaxLength(500);
                entity.Property(e => e.LinkYoutube).HasMaxLength(250);
                entity.Property(e => e.PhonePartner).HasMaxLength(15);
                entity.Property(e => e.LinkFacebook).HasMaxLength(250);
                entity.Property(e => e.EmailPartner).HasMaxLength(150);
                entity.Property(e => e.CompanyNameVn).HasMaxLength(250);
                entity.Property(e => e.CompanyNameEn).HasMaxLength(250);
                entity.Property(e => e.PhoneNewspapers).HasMaxLength(15);
                entity.Property(e => e.EmailNewspapers).HasMaxLength(150);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGPageSection>(entity =>
            {
                entity.ToTable("mg_page_sections");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Order);
                entity.HasIndex(e => e.PageId);
                entity.HasIndex(e => e.SectionId);
                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PageSections)
                    .HasForeignKey(d => d.PageId)
                    .HasConstraintName("FK_PageSections_PageId");
                entity.HasOne(d => d.Section)
                    .WithMany(p => p.PageSections)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_PageSections_SectionId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGCategoryAnnounced>(entity =>
            {
                entity.ToTable("mg_category_announceds");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(d => d.Parent)
                   .WithMany(p => p.Categories)
                   .HasForeignKey(d => d.ParentId)
                   .HasConstraintName("FK_CategoryAnnounceds_ParentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGRecruitment>(entity =>
            {
                entity.ToTable("mg_recruitments");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.TitleVn);
                entity.Property(e => e.TitleVn).HasMaxLength(250);
                entity.Property(e => e.TitleEn).HasMaxLength(250);
                entity.Property(e => e.SummaryVn).HasMaxLength(150);
                entity.Property(e => e.SummaryEn).HasMaxLength(150);
                entity.Property(e => e.ExperienceVn).HasMaxLength(100);
                entity.Property(e => e.ExperienceEn).HasMaxLength(100);
                entity.Property(e => e.ContactVn).HasMaxLength(250);
                entity.Property(e => e.ContactEn).HasMaxLength(250);
                entity.Property(e => e.Image).HasMaxLength(250);
                entity.HasOne(e => e.Rank)
                   .WithMany(p => p.Recruitment)
                   .HasForeignKey(e => e.RankId)
                   .HasConstraintName("FK_MGRecruitmentRank_RankId");
                entity.HasOne(e => e.Degree)
                   .WithMany(p => p.Recruitment)
                   .HasForeignKey(e => e.DegreeId)
                   .HasConstraintName("FK_MGRecruitmentDegree_DegreeId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGJob>(entity =>
            {
                entity.ToTable("mg_jobs");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(e => e.JobLabel)
                   .WithMany(p => p.Job)
                   .HasForeignKey(e => e.JobLabelId)
                   .HasConstraintName("FK_MGJob_JobLabelId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGRecruitmentJob>(entity =>
            {
                entity.ToTable("mg_recruitment_job");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.MGRecruitment)
                   .WithMany(p => p.RecruitmentJob)
                   .HasForeignKey(e => e.RecruitmentId)
                   .HasConstraintName("FK_MGRecruitmentJobV2_RecruitmentId");
                entity.HasOne(e => e.MGJob)
                   .WithMany(p => p.RecruitmentJob)
                   .HasForeignKey(e => e.JobId)
                   .HasConstraintName("FK_MGRecruitmentJobV2_JobId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGWelfare>(entity =>
            {
                entity.ToTable("mg_welfares");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.Property(e => e.Icon).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGRecruitmentWelfare>(entity =>
            {
                entity.ToTable("mg_recruitment_welfare");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.MGRecruitment)
                   .WithMany(p => p.RecruitmentWelfare)
                   .HasForeignKey(e => e.RecruitmentId)
                   .HasConstraintName("FK_MGRecruitmentWelfare_RecruitmentId");
                entity.HasOne(e => e.MGWelfare)
                   .WithMany(p => p.RecruitmentWelfare)
                   .HasForeignKey(e => e.WelfareId)
                   .HasConstraintName("FK_MGRecruitmentWelfare_WelfareId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGProfile>(entity =>
            {
                entity.ToTable("mg_profile");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.Phone);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.File).HasMaxLength(250);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.HasOne(d => d.Recruitment)
                    .WithMany(p => p.Profile)
                    .HasForeignKey(d => d.RecruitmentId)
                    .HasConstraintName("FK_MGProfile_RecruitmentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGRank>(entity =>
            {
                entity.ToTable("mg_ranks");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGDegree>(entity =>
            {
                entity.ToTable("mg_degrees");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGJobLabel>(entity =>
            {
                entity.ToTable("mg_job_labels");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGPosition>(entity =>
            {
                entity.ToTable("mg_positions");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NameVn);
                entity.Property(e => e.NameVn).HasMaxLength(250);
                entity.Property(e => e.NameEn).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MGRecruitmentPosition>(entity =>
            {
                entity.ToTable("mg_recruitment_position");
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.MGRecruitment)
                   .WithMany(p => p.RecruitmentPosition)
                   .HasForeignKey(e => e.RecruitmentId)
                   .HasConstraintName("FK_MGRecruitmentPosition_RecruitmentId");
                entity.HasOne(e => e.MGPosition)
                   .WithMany(p => p.RecruitmentPosition)
                   .HasForeignKey(e => e.PositionId)
                   .HasConstraintName("FK_MGRecruitmentPosition_PositionId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            //Meey Order
            modelBuilder.Entity<MOOrder>(entity =>
            {
                entity.ToTable("MOOrders");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.OrderId).IsUnique();
                entity.Property(e => e.OrderId).HasMaxLength(100);
                entity.Property(e => e.MeeyId).IsRequired().HasMaxLength(100);
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.MOSaleOrders)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_MOSaleOrders_SaleId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.MOSupportOrders)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_MOSupportOrders_SupportId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<ServiceUserEntity>(entity =>
            {
                entity.ToTable("ServiceUsers");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ServiceId);
                entity.HasIndex(e => e.ComboId);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<PriceConfigUserEntity>(entity =>
            {
                entity.ToTable("PriceConfigUsers");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PriceConfigServiceId);
                entity.HasIndex(e => e.PriceConfigComboId);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // MeeyCRM
            modelBuilder.Entity<MCRMCompany>(entity =>
            {
                entity.ToTable("mcrm_companies");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Phone);
                entity.HasIndex(c => c.Email);
                entity.HasIndex(c => c.Leader);
                entity.HasIndex(c => c.Status);
                entity.HasIndex(c => c.LeaderPhone);
                entity.HasIndex(c => c.LeaderEmail);
                entity.HasIndex(c => c.Name).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Leader).HasMaxLength(250);
                entity.Property(e => e.TaxCode).HasMaxLength(20);
                entity.Property(e => e.Website).HasMaxLength(250);
                entity.Property(e => e.Address).HasMaxLength(2000);
                entity.Property(e => e.LeaderPhone).HasMaxLength(15);
                entity.Property(e => e.LeaderEmail).HasMaxLength(250);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(15);
                entity.HasOne(d => d.City)
                    .WithMany(p => p.MCRMCompanies)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_MCRMCompanies_CityId");
                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.MCRMCompanies)
                    .HasForeignKey(d => d.WardId)
                    .HasConstraintName("FK_MCRMCompanies_WardId");
                entity.HasOne(d => d.District)
                    .WithMany(p => p.MCRMCompanies)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_MCRMCompanies_DistrictId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCallLog>(entity =>
            {
                entity.ToTable("mcrm_call_logs");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Type);
                entity.HasIndex(c => c.Phone);
                entity.HasIndex(c => c.Status);
                entity.HasIndex(c => c.CallId);
                entity.HasIndex(c => c.CallTime);
                entity.HasIndex(c => c.CallStatus);
                entity.HasIndex(c => c.Recordingfile);
                entity.HasIndex(c => c.CallTimeString);
                entity.HasIndex(c => c.UniqueId).IsUnique();
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.CallId).HasMaxLength(50);
                entity.Property(e => e.Secret).HasMaxLength(50);
                entity.Property(e => e.UniqueId).HasMaxLength(50);
                entity.Property(e => e.CallTimeString).HasMaxLength(50);
                entity.Property(e => e.Recordingfile).HasMaxLength(500);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MCRMCallLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MCRMCallLogs_UserId");
                entity.HasOne(d => d.MCRMCustomer)
                     .WithMany(p => p.MCRMCallLogs)
                     .HasForeignKey(d => d.MCRMCustomerId)
                     .HasConstraintName("FK_MCRMCallLogs_MCRMCustomerId");
                entity.HasOne(d => d.MCRMCustomerLead)
                     .WithMany(p => p.MCRMCallLogs)
                     .HasForeignKey(d => d.MCRMCustomerLeadId)
                     .HasConstraintName("FK_MCRMCallLogs_MCRMCustomerLeadId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCallLogEvent>(entity =>
            {
                entity.ToTable("mcrm_call_log_events");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.type).HasMaxLength(500);
                entity.Property(e => e.phone).HasMaxLength(50);
                entity.Property(e => e.callid).HasMaxLength(50);
                entity.Property(e => e.secret).HasMaxLength(50);
                entity.Property(e => e.status).HasMaxLength(250);
                entity.Property(e => e.billsec).HasMaxLength(250);
                entity.Property(e => e.calldate).HasMaxLength(50);
                entity.Property(e => e.duration).HasMaxLength(250);
                entity.Property(e => e.extension).HasMaxLength(500);
                entity.Property(e => e.eventString).HasMaxLength(50);
                entity.Property(e => e.recordingfile).HasMaxLength(500);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomer>(entity =>
            {
                entity.ToTable("mcrm_customers");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Name);
                entity.HasIndex(c => c.WardId);
                entity.HasIndex(c => c.CityId);
                entity.HasIndex(c => c.SaleId);
                entity.HasIndex(c => c.RootId);
                entity.HasIndex(c => c.IsActive);
                entity.HasIndex(c => c.IsDelete);
                entity.HasIndex(c => c.UserName);
                entity.HasIndex(c => c.SourceId);
                entity.HasIndex(c => c.SupportId);
                entity.HasIndex(c => c.DistrictId);
                entity.HasIndex(c => c.ExpiredDate);
                entity.HasIndex(c => c.CreatedDate);
                entity.HasIndex(c => c.UpdatedDate);
                entity.HasIndex(c => c.CustomerType);
                entity.HasIndex(c => c.Code).IsUnique();
                entity.HasIndex(c => c.LastTimeSupport);
                entity.HasIndex(c => c.DistributionType);
                entity.HasIndex(c => c.CustomerStoreType);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.HasIndex(c => c.CustomerStatusType);
                entity.HasIndex(c => c.CustomerActivityType);
                entity.HasIndex(c => c.CustomerPotentialType);
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.Avatar).HasMaxLength(250);
                entity.Property(e => e.Affiliate).HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(1000);
                entity.Property(e => e.UserName).HasMaxLength(250);
                entity.Property(e => e.CustomerRef).HasMaxLength(250);
                entity.Property(e => e.InvoiceName).HasMaxLength(250);
                entity.Property(e => e.AccountSource).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.InvoiceTaxCode).HasMaxLength(50);
                entity.Property(e => e.CustomerRefCode).HasMaxLength(20);
                entity.Property(e => e.InvoiceAddress).HasMaxLength(1000);
                entity.HasOne(d => d.Root)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.RootId)
                    .HasConstraintName("FK_MCRMCustomers_RootId");
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleMCRMCustomers)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_MCRMCustomers_SaleId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.SupportMCRMCustomers)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_MCRMCustomers_SupportId");
                entity.HasOne(d => d.City)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_MCRMCustomers_CityId");
                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.WardId)
                    .HasConstraintName("FK_MCRMCustomers_WardId");
                entity.HasOne(d => d.District)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_MCRMCustomers_DistrictId");
                entity.HasOne(d => d.Source)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK_MCRMCustomers_SourceId");
                entity.HasOne(d => d.MCRMCompany)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.MCRMCompanyId)
                    .HasConstraintName("FK_MCRMCustomers_MCRMCompanyId");
                entity.HasOne(d => d.Department)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_MCRMCustomers_DepartmentId");
                entity.HasOne(d => d.PrevSale)
                    .WithMany(p => p.PrevSaleMCRMCustomers)
                    .HasForeignKey(d => d.PrevSaleId)
                    .HasConstraintName("FK_PrevMCRMCustomers_PrevSaleId");
                entity.HasOne(d => d.CustomerGroup)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.CustomerGroupId)
                    .HasConstraintName("FK_MCRMCustomers_CustomerGroupId");
                entity.HasOne(d => d.MCRMIframeContract)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.MCRMIframeContractId)
                    .HasConstraintName("FK_MCRMCustomers_MCRMIframeContractId");
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MCRMCustomers)
                    .HasForeignKey(d => d.InterestedProductId)
                    .HasConstraintName("FK_MCRMCustomers_InterestedProductId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerLead>(entity =>
            {
                entity.ToTable("mcrm_customer_leads");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Name);
                entity.HasIndex(c => c.Phone);
                entity.HasIndex(c => c.Email);
                entity.HasIndex(c => c.CityId);
                entity.HasIndex(c => c.SaleId);
                entity.HasIndex(c => c.IsActive);
                entity.HasIndex(c => c.IsDelete);
                entity.HasIndex(c => c.SupportId);
                entity.HasIndex(c => c.DistrictId);
                entity.HasIndex(c => c.CustomerType);
                entity.HasIndex(c => c.LastTimeSupport);
                entity.HasIndex(c => c.MeeyId).IsUnique();
                entity.HasIndex(c => c.CustomerPotentialType);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.IdCard).HasMaxLength(20);
                entity.Property(e => e.MeeyId).HasMaxLength(50);
                entity.Property(e => e.Avatar).HasMaxLength(250);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.AccountSource).HasMaxLength(50);
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleMCRMCustomersLeads)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_MCRMCustomersLeads_SaleId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.SupportMCRMCustomersLeads)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_MCRMCustomersLeads_SupportId");
                entity.HasOne(d => d.City)
                    .WithMany(p => p.MCRMCustomersLeads)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_MCRMCustomersLeads_CityId");
                entity.HasOne(d => d.Ward)
                    .WithMany(p => p.MCRMCustomersLeads)
                    .HasForeignKey(d => d.WardId)
                    .HasConstraintName("FK_MCRMCustomersLeads_WardId");
                entity.HasOne(d => d.District)
                    .WithMany(p => p.MCRMCustomersLeads)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_MCRMCustomersLeads_DistrictId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerNote>(entity =>
            {
                entity.ToTable("mcrm_customer_notes");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Phone);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MCRMCustomerNotes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MCRMCustomerNotes_UserId");
                entity.HasOne(d => d.MCRMCustomer)
                     .WithMany(p => p.MCRMCustomerNotes)
                     .HasForeignKey(d => d.MCRMCustomerId)
                     .HasConstraintName("FK_MCRMCustomerNotes_MCRMCustomerId");
                entity.HasOne(d => d.MCRMCustomerLead)
                     .WithMany(p => p.MCRMCustomerNotes)
                     .HasForeignKey(d => d.MCRMCustomerLeadId)
                     .HasConstraintName("FK_MCRMCustomerNotes_MCRMCustomerLeadId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerGroup>(entity =>
            {
                entity.ToTable("mcrm_customer_groups");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Name);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMEmailTemplate>(entity =>
            {
                entity.ToTable("mcrm_email_templates");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Title);
                entity.HasIndex(c => c.Name).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
                entity.HasOne(d => d.EmailTemplateWrapper)
                    .WithMany(p => p.MCRMEmailTemplates)
                    .HasForeignKey(d => d.EmailTemplateWrapperId)
                    .HasConstraintName("FK_MCRMEmailTemplates_EmailTemplateWrapperId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerContact>(entity =>
            {
                entity.ToTable("mcrm_customer_contacts");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Name);
                entity.HasIndex(c => c.Phone);
                entity.HasIndex(c => c.Email);
                entity.HasIndex(c => c.AllowView);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.HasOne(d => d.MCRMCustomer)
                    .WithMany(p => p.MCRMCustomerContacts)
                    .HasForeignKey(d => d.MCRMCustomerId)
                    .HasConstraintName("FK_MCRMCustomerContacts_MCRMCustomerId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMIframeContract>(entity =>
            {
                entity.ToTable("mcrm_iframe_contracts");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Domain);
                entity.HasIndex(c => c.ContractName);
                entity.HasIndex(c => c.CompanyName);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Domain).HasMaxLength(250);
                entity.Property(e => e.RefCode).HasMaxLength(20);
                entity.Property(e => e.IframeCode).HasMaxLength(250);
                entity.Property(e => e.CompanyName).HasMaxLength(500);
                entity.Property(e => e.Attachments).HasMaxLength(500);
                entity.Property(e => e.PartnerName).HasMaxLength(250);
                entity.Property(e => e.ContractName).HasMaxLength(500);
                entity.Property(e => e.PartnerMeeyId).HasMaxLength(250);
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.MCRMIframeContracts)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_MCRMIframeContracts_SaleId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMIframeHistory>(entity =>
            {
                entity.ToTable("mcrm_iframe_histories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Action).HasMaxLength(500);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.HasOne(d => d.MCRMIframeContract)
                    .WithMany(p => p.MCRMIframeHistories)
                    .HasForeignKey(d => d.MCRMIframeContractId)
                    .HasConstraintName("FK_MCRMIframeHistories_MCRMIframeContractId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerHistory>(entity =>
            {
                entity.ToTable("mcrm_customer_histories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Action);
                entity.HasIndex(c => c.StatusAfter);
                entity.HasIndex(c => c.StatusBefore);
                entity.Property(e => e.CreatedByName).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.MCRMCustomer)
                     .WithMany(p => p.MCRMCustomerHistories)
                     .HasForeignKey(d => d.MCRMCustomerId)
                     .HasConstraintName("FK_MCRMCustomerHistories_MCRMCustomerId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMSaleAssignConfigHistory>(entity =>
            {
                entity.ToTable("mcrm_sale_asign_config_histories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Action);
                entity.HasOne(d => d.User)
                     .WithMany(p => p.MCRMSaleAssignConfigHistories)
                     .HasForeignKey(d => d.UserId)
                     .HasConstraintName("FK_MCRMSaleAssignConfigHistories_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerRequest>(entity =>
            {
                entity.ToTable("mcrm_customer_requests");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Code).IsUnique();
                entity.Property(e => e.Ids).HasMaxLength(500);
                entity.Property(e => e.Ids).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(1000);
                entity.Property(e => e.Email).HasMaxLength(1000);
                entity.Property(e => e.Reason).HasMaxLength(500);
                entity.Property(e => e.ApproveReason).HasMaxLength(500);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MCRMCustomerRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MCRMCustomerRequests_UserId");
                entity.HasOne(d => d.Sale)
                    .WithMany(p => p.SaleMCRMCustomerRequests)
                    .HasForeignKey(d => d.SaleId)
                    .HasConstraintName("FK_SaleMCRMCustomerRequests_SaleId");
                entity.HasOne(d => d.Support)
                    .WithMany(p => p.SupportMCRMCustomerRequests)
                    .HasForeignKey(d => d.SupportId)
                    .HasConstraintName("FK_SupportMCRMCustomerRequests_SupportId");
                entity.HasOne(d => d.UserApprove)
                    .WithMany(p => p.MCRMCustomerRequestApproves)
                    .HasForeignKey(d => d.UserApproveId)
                    .HasConstraintName("FK_MCRMCustomerRequestApproves_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerNoteEmail>(entity =>
            {
                entity.ToTable("mcrm_customer_note_emails");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Status);
                entity.Property(e => e.Title).HasMaxLength(500);
                entity.Property(e => e.EmailCc).HasMaxLength(250);
                entity.Property(e => e.EmailTo).HasMaxLength(2000);
                entity.Property(e => e.EmailBcc).HasMaxLength(250);
                entity.Property(e => e.EmailFrom).HasMaxLength(250);
                entity.Property(e => e.Attachments).HasMaxLength(2000);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MCRMCustomerNoteEmails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MCRMCustomerNoteEmails_UserId");
                entity.HasOne(d => d.MCRMCustomer)
                     .WithMany(p => p.MCRMCustomerNoteEmails)
                     .HasForeignKey(d => d.MCRMCustomerId)
                     .HasConstraintName("FK_MCRMCustomerNoteEmails_MCRMCustomerId");
                entity.HasOne(d => d.MCRMCustomerLead)
                     .WithMany(p => p.MCRMCustomerNoteEmails)
                     .HasForeignKey(d => d.MCRMCustomerLeadId)
                     .HasConstraintName("FK_MCRMCustomerNoteEmails_MCRMCustomerLeadId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerAttachment>(entity =>
            {
                entity.ToTable("mcrm_customer_attachments");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Name);
                entity.HasIndex(c => c.File);
                entity.Property(e => e.Name).HasMaxLength(500);
                entity.Property(e => e.File).HasMaxLength(500);
                entity.HasOne(d => d.MCRMCustomerNoteEmail)
                     .WithMany(p => p.MCRMCustomerAttachments)
                     .HasForeignKey(d => d.MCRMCustomerNoteEmailId)
                     .HasConstraintName("FK_MCRMCustomerAttachments_MCRMCustomerNoteEmailId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMCustomerLeadHistory>(entity =>
            {
                entity.ToTable("mcrm_customer_lead_histories");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Action);
                entity.HasIndex(c => c.StatusAfter);
                entity.HasIndex(c => c.StatusBefore);
                entity.Property(e => e.CreatedByName).IsRequired().HasMaxLength(250);
                entity.HasOne(d => d.MCRMCustomerLead)
                     .WithMany(p => p.MCRMCustomerLeadHistories)
                     .HasForeignKey(d => d.MCRMCustomerLeadId)
                     .HasConstraintName("FK_MCRMCustomerLeadHistories_MCRMCustomerLeadId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMSaleAssignProductQueue>(entity =>
            {
                entity.ToTable("mcrm_sale_assign_products");
                entity.HasIndex(c => c.Email);
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.FullName);
                entity.HasIndex(c => c.DepartmentId);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.FullName).HasMaxLength(250);
                entity.Property(e => e.Products).HasMaxLength(500);
                entity.HasOne(d => d.User)
                     .WithMany(p => p.SaleAssignProductQueues)
                     .HasForeignKey(d => d.UserId)
                     .HasConstraintName("FK_SaleAssignProductQueues_UserId");
                entity.HasOne(d => d.Department)
                     .WithMany(p => p.SaleAssignProductQueues)
                     .HasForeignKey(d => d.DepartmentId)
                     .HasConstraintName("FK_SaleAssignProductQueues_DepartmentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMSaleAssignConfig>(entity =>
            {
                entity.ToTable("mcrm_assign_sale_config");
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            //MeeyAffiliate
            modelBuilder.Entity<MAFAffiliateRequest>(entity =>
            {
                entity.ToTable("maf_affiliate_requests");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.Code).IsUnique();
                entity.Property(e => e.RefCode).HasMaxLength(250);
                entity.Property(e => e.MeeyId).HasMaxLength(250);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.Property(e => e.Phone).HasMaxLength(250);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.Note).HasMaxLength(250);
                entity.Property(e => e.Reason).HasMaxLength(500);
                entity.Property(e => e.Search).HasMaxLength(250);
                entity.Property(e => e.RefCurrentMeeyId).HasMaxLength(250);
                entity.Property(e => e.RefCurrentName).HasMaxLength(250);
                entity.Property(e => e.RefCurrentPhone).HasMaxLength(250);
                entity.Property(e => e.RefCurrentRankName).HasMaxLength(250);
                entity.Property(e => e.RefChangeMeeyId).HasMaxLength(250);
                entity.Property(e => e.RefChangeName).HasMaxLength(250);
                entity.Property(e => e.RefChangePhone).HasMaxLength(250);
                entity.Property(e => e.RefChangeRankName).HasMaxLength(250);
                entity.Property(e => e.ApproveReason).HasMaxLength(500);
                entity.Property(e => e.File).HasMaxLength(500);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.HasOne(d => d.User)
                    .WithMany(p => p.MAFAffiliateRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_MAFAffiliateRequests_UserId");
                entity.HasOne(d => d.UserApprove)
                    .WithMany(p => p.MAFAffiliateRequestApproves)
                    .HasForeignKey(d => d.UserApproveId)
                    .HasConstraintName("FK_MAFAffiliateRequestApproves_UserId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            // PriceProperty
            modelBuilder.Entity<PriceProperty>(entity =>
            {
                entity.ToTable("price_properties");
                entity.HasKey(e => e.Id);
                entity.HasIndex(c => c.FloorId);
                entity.HasIndex(c => c.ProjectId);
                entity.HasIndex(c => c.BuildingId);
                entity.HasIndex(c => c.PropertyId);
                entity.Property(e => e.FloorId).HasMaxLength(250);
                entity.Property(e => e.FloorName).HasMaxLength(250);
                entity.Property(e => e.ProjectId).HasMaxLength(250);
                entity.Property(e => e.PropertyId).HasMaxLength(250);
                entity.Property(e => e.BuildingId).HasMaxLength(250);
                entity.Property(e => e.ProjectName).HasMaxLength(250);
                entity.Property(e => e.BuildingName).HasMaxLength(250);
                entity.Property(e => e.PropertyName).HasMaxLength(250);
                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.UnitPrice).HasMaxLength(250);
                entity.Property(e => e.TotalPrice).HasMaxLength(250);
                entity.Property(e => e.RentalPrice).HasMaxLength(250);
                entity.Property(e => e.SellingPrice).HasMaxLength(250);
                entity.Property(e => e.SquareMeters).HasMaxLength(250);
                entity.Property(e => e.SquareMetersType).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });

            modelBuilder.Entity<MCRMSaleAssignQueue>(entity =>
            {
                entity.ToTable("mcrm_sale_assign");
                entity.HasIndex(c => c.Email);
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.FullName);
                entity.HasIndex(c => c.DepartmentId);
                entity.Property(e => e.Email).HasMaxLength(250);
                entity.Property(e => e.FullName).HasMaxLength(250);
                entity.HasOne(d => d.User)
                     .WithMany(p => p.SaleAssignQueues)
                     .HasForeignKey(d => d.UserId)
                     .HasConstraintName("FK_SaleAssignQueues_UserId");
                entity.HasOne(d => d.Department)
                     .WithMany(p => p.SaleAssignQueues)
                     .HasForeignKey(d => d.DepartmentId)
                     .HasConstraintName("FK_SaleAssignQueues_DepartmentId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MCRMSaleHistory>(entity =>
            {
                entity.ToTable("mcrm_sale_histories");
                entity.HasIndex(c => c.UserId);
                entity.HasIndex(c => c.MCRMCustomerId);
                entity.Property(e => e.Detail).HasMaxLength(250);
                entity.HasOne(d => d.User)
                     .WithMany(p => p.MCRMSaleHistories)
                     .HasForeignKey(d => d.UserId)
                     .HasConstraintName("FK_MCRMSaleHistories_UserId");
                entity.HasOne(d => d.MCRMCustomer)
                     .WithMany(p => p.MCRMSaleHistories)
                     .HasForeignKey(d => d.MCRMCustomerId)
                     .HasConstraintName("FK_MCRMSaleHistories_MCRMCustomerId");
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
            modelBuilder.Entity<MSMetaSeoTemplate>(entity =>
            {
                entity.ToTable("ms_meta_seo_template");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(250);
                entity.HasOne(d => d.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedBy);
                entity.HasOne(d => d.UpdatedByUser).WithMany().HasForeignKey(c => c.UpdatedBy);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var builder = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{environment}.json", true, true)
                            .AddEnvironmentVariables();
                var configuration = builder.Build();

                var appSettingsSection = configuration.GetSection("AppSettings");
                var appSettings = appSettingsSection.Get<AppSettings>();
                var connectionString = configuration.GetConnectionString(nameof(MeeyAdminContext));

                switch (appSettings.DbType)
                {
                    case "mysql":
                        {
                            var connection = new MySqlConnectionStringBuilder(connectionString)
                            {
                                MinimumPoolSize = 5,
                                MaximumPoolSize = 600,
                            };
                            optionsBuilder.UseMySql(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                    case "pgsql":
                        {
                            var connection = new NpgsqlConnectionStringBuilder(connectionString)
                            {
                                MinPoolSize = 5,
                                MaxPoolSize = 600,
                            };
                            optionsBuilder.UseNpgsql(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                    case "sql":
                        {
                            var connection = new SqlConnectionStringBuilder(connectionString)
                            {
                                MinPoolSize = 5,
                                MaxPoolSize = 600,
                            };
                            optionsBuilder.UseSqlServer(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                }
                base.OnConfiguring(optionsBuilder);
            }
        }

        public override void Dispose()
        {
            MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
            base.Dispose();
        }
    }
    public partial class MeeyAdminStagContext : MeeyAdminContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{EnvironmentType.Stag}.json", true, true)
                            .AddEnvironmentVariables();
                var configuration = builder.Build();

                var appSettingsSection = configuration.GetSection("AppSettings");
                var appSettings = appSettingsSection.Get<AppSettings>();
                var connectionString = configuration.GetConnectionString(nameof(MeeyAdminContext));

                switch (appSettings.DbType)
                {
                    case "mysql":
                        {
                            var connection = new MySqlConnectionStringBuilder(connectionString)
                            {
                                MinimumPoolSize = 5,
                                MaximumPoolSize = 600,
                            };
                            optionsBuilder.UseMySql(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                    case "pgsql":
                        {
                            var connection = new NpgsqlConnectionStringBuilder(connectionString)
                            {
                                MinPoolSize = 5,
                                MaxPoolSize = 600,
                            };
                            optionsBuilder.UseNpgsql(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                    case "sql":
                        {
                            var connection = new SqlConnectionStringBuilder(connectionString)
                            {
                                MinPoolSize = 5,
                                MaxPoolSize = 600,
                            };
                            optionsBuilder.UseSqlServer(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                }
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
    public partial class MeeyAdminProdContext : MeeyAdminContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{EnvironmentType.Production}.json", true, true)
                            .AddEnvironmentVariables();
                var configuration = builder.Build();

                var appSettingsSection = configuration.GetSection("AppSettings");
                var appSettings = appSettingsSection.Get<AppSettings>();
                var connectionString = configuration.GetConnectionString(nameof(MeeyAdminContext));

                switch (appSettings.DbType)
                {
                    case "mysql":
                        {
                            var connection = new MySqlConnectionStringBuilder(connectionString)
                            {
                                MinimumPoolSize = 5,
                                MaximumPoolSize = 600,
                            };
                            optionsBuilder.UseMySql(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                    case "pgsql":
                        {
                            var connection = new NpgsqlConnectionStringBuilder(connectionString)
                            {
                                MinPoolSize = 5,
                                MaxPoolSize = 600,
                            };
                            optionsBuilder.UseNpgsql(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                    case "sql":
                        {
                            var connection = new SqlConnectionStringBuilder(connectionString)
                            {
                                MinPoolSize = 5,
                                MaxPoolSize = 600,
                            };
                            optionsBuilder.UseSqlServer(connection.ToString(), options => options.EnableRetryOnFailure());

                        }
                        break;
                }
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
