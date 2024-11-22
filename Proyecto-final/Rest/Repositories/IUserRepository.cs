using Rest.Models;

namespace Rest.Repositories;

public interface IUserRepository{
    Task<UserModel> GetByIdAsync(int Id, CancellationToken cancellationToken);
    Task<IEnumerable<UserModel>> GetByName(string name, int PageIndex, int PageSize, string orderBy, CancellationToken cancellationToken);
    public Task DeleteByIdAsync(int Id, CancellationToken cancellationToken);
    public Task<UserModel> CreateAsync(string name, List<string> persona, CancellationToken cancellationToken);
    public Task UpdateUserAsync(int id, string name, List<string> persona, CancellationToken cancellationToken);
}