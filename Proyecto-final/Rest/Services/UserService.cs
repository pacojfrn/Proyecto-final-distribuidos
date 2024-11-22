using Microsoft.AspNetCore.Mvc;
using Rest.Dtos;
using Rest.Models;
using Rest.Repositories;
using Rest.Mappers;

namespace Rest.Services;

public class UserService : IUserService{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository){
        _userRepository = userRepository;
    }

    public async Task<UserModel> GetUserByIdAsync(int id, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if(user is not null){
            return user; // Cambiado para asegurarse que retorna un DTO.
        }
        return null; // Retorna null explícitamente si no encuentra el usuario.
    }

    public async Task<IEnumerable<UserModel>> GetUserByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken){
        var users = await _userRepository.GetByName(name, pageIndex, pageSize, orderBy, cancellationToken);
        return users;
    }

public async Task<UserModel> CreateUserAsync(string name, List<string> persona, CancellationToken cancellationToken)
{
    // Crear un DTO (o tipo esperado por el repositorio)
    var createUserRequest = new CreateUserRequest { Name = name, Persona = persona };
    
    // Llamar al repositorio con el DTO adecuado y el CancellationToken
    var createdUser = await _userRepository.CreateAsync(createUserRequest.Name, createUserRequest.Persona, cancellationToken);
    
    // Convertir el DTO a un UserModel si es necesario
    return createdUser; // Asegúrate de que 'ToModel' sea un método que mapea el DTO a UserModel
}

    public async Task UpdateUserAsync(int Id, string name, List<string> persona, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(Id, cancellationToken);
        if(user is not null){
            await _userRepository.UpdateUserAsync(Id, name, persona, cancellationToken);
        }
    }

    public async Task DeleteUserByIdAsync(int Id, CancellationToken cancellationToken){
        await _userRepository.DeleteByIdAsync(Id, cancellationToken);
    }

    public async Task<UserModel?> PatchUserAsync(int id, PatchUserRequest patchuser, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if(user == null){
            return null;
        }
        if(!string.IsNullOrEmpty(patchuser.Name)){
            user.Name = patchuser.Name;
        }
        if(patchuser.Persona != null && patchuser.Persona.Any()){
            user.Persona = patchuser.Persona;
        }
        await _userRepository.UpdateUserAsync(user.Id, user.Name, user.Persona, cancellationToken);
        return user;
    }
}

