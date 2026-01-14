using Poolup.Core.Entities.Authentication;

namespace Poolup.Core.Interfaces;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id);
    Task<List<User>> GetAllAsync();
}
