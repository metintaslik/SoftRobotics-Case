using Microsoft.EntityFrameworkCore;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.infrastructure.Persistance;

public partial class SoftRoboticsDbContext : DbContext, ISoftRoboticsDbContext
{
    public SoftRoboticsDbContext()
    {
    }

    public SoftRoboticsDbContext(DbContextOptions<SoftRoboticsDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<UserValidate> UserValidates { get; set; }

    public async Task<int> SaveToDbAsync(CancellationToken cancellationToken = default) => await SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).UseIdentityColumn().HasColumnName("UserID");
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Password).IsFixedLength(true).HasMaxLength(64);
            entity.Property(e => e.Mail).HasMaxLength(100);
            entity.Property(p => p.Created).HasColumnType("datetime");
            entity.Property(p => p.LastModified).HasColumnType("datetime");
            entity.Property(p => p.IsActive).HasColumnType("bit");

            entity.HasMany(d => d.UserValidates)
                  .WithOne(d => d.User)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK_UserValidate_User");
        });

        modelBuilder.Entity<UserValidate>(entity =>
        {
            entity.ToTable("UserValidate");

            entity.HasKey(e => e.UserValidateId);
            entity.Property(e => e.UserValidateId).UseIdentityColumn().HasColumnName("UserValidateID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.HashUUID).HasMaxLength(64).IsUnicode(false).HasColumnName("HashUUID");
            entity.Property(p => p.Created).HasColumnType("datetime");
            entity.Property(p => p.LastModified).HasColumnType("datetime");
            entity.Property(p => p.IsActive).HasColumnType("bit");

            entity.HasOne(d => d.User).WithMany(p => p.UserValidates)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserValidate_User");
        });

        base.OnModelCreating(modelBuilder);
    }
}
