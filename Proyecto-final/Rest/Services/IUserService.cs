using Rest.Models;
using Rest.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Rest.Services;

public interface IUserService{
    public Task<UserModel> GetUserByIdAsync(int Id, CancellationToken cancellationtoken);
    public Task<IEnumerable<UserModel>> GetUserByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken);
    public Task DeleteUserByIdAsync(int Id, CancellationToken cancellationToken);
    public Task UpdateUserAsync(int Id, string name, List<string> persona, CancellationToken cancellationToken);
    public Task<UserModel> CreateUserAsync(string name, List<string> persona, CancellationToken cancellationToken);
    public Task<UserModel> PatchUserAsync(int id, PatchUserRequest patchuser, CancellationToken cancellationToken);
}