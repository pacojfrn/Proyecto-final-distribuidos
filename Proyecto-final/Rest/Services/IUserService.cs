using Rest.Models;
using Rest.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Rest.Services;

public interface IUserService{
    public Task<UserPerModel> GetUserByIdAsync(int Id, CancellationToken cancellationtoken);
    public Task<IEnumerable<UserPerModel>> GetUserByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken);
    public Task DeleteUserByIdAsync(int Id, CancellationToken cancellationToken);
    public Task UpdateUserAsync(int Id, string name, string persona, CancellationToken cancellationToken);
    public Task<UserPerModel> CreateUserAsync(string name, string persona, CancellationToken cancellationToken);
    public Task<UserPerModel> PatchUserAsync(int id, PatchUserRequest patchuser, CancellationToken cancellationToken);
}