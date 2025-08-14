using FlowManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.Json;

namespace FlowManager.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid,
        IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flow> Flows => Set<Flow>();
        public DbSet<Component> Components => Set<Component>();
        public DbSet<Step> Steps => Set<Step>();
        public DbSet<FlowStep> FlowSteps => Set<FlowStep>();
        public DbSet<FormTemplate> FormTemplates => Set<FormTemplate>();
        public DbSet<FormResponse> FormResponses => Set<FormResponse>();
        public DbSet<FormTemplateComponent> FormTemplateComponents => Set<FormTemplateComponent>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            UniqueUserEmailConstraintConfiguration(builder);
            UniqueRolenameConstraintConfiguration(builder);

            UserRoleRelationshipConfiguration(builder);

            JSONBConfiguration(builder);
        }

        private void UserRoleRelationshipConfiguration(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("AspNetUsers");


                entity.HasMany(u => u.Roles)
                      .WithOne(ur => ur.User)
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired();
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable("AspNetRoles");

                entity.HasMany(r => r.Users)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired();
            });
        }

        private void JSONBConfiguration(ModelBuilder builder)
        {
            builder.Entity<Component>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Properties)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                          v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions)null))
                      .HasColumnType("jsonb"); // PostgreSQL jsonb type
            });

            builder.Entity<FormResponse>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ResponseFields)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                          v => JsonSerializer.Deserialize<Dictionary<Guid, object>>(v, (JsonSerializerOptions)null))
                      .HasColumnType("jsonb"); // PostgreSQL jsonb type
            });
        }

        private void UniqueRolenameConstraintConfiguration(ModelBuilder builder)
        {
            builder.Entity<Role>()
                    .HasIndex(r => r.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_AspNetRoles_Name");
            builder.Entity<Role>()
                    .HasIndex(r => r.NormalizedName)
                    .IsUnique()
                    .HasDatabaseName("IX_AspNetRoles_NormalizedName");
        }

        private void UniqueUserEmailConstraintConfiguration(ModelBuilder builder)
        {
            builder.Entity<User>()
                    .HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_AspNetUsers_Email");

            builder.Entity<User>()
                    .HasIndex(u => u.NormalizedEmail)
                    .IsUnique()
                    .HasDatabaseName("IX_AspNetUsers_NormalizedEmail");
        }
    }
}
