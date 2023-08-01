using Microsoft.EntityFrameworkCore;
using softrobotics.auth.domain.Entity;

namespace softrobotics.auth.application.Common.Interface;

public interface ISoftRoboticsDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserValidate> UserValidates { get; }

    Task<int> SaveToDbAsync(CancellationToken cancellationToken = default);
}