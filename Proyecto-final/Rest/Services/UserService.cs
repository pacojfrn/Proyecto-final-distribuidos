using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rest.Dtos;
using Rest.Models;
using Rest.Repositories;
using Rest.Mappers;
using Rest.Infraestructure.Entities;

namespace Rest.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPerRepository _perRepository;

    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IPerRepository perRepository)
    {
        _userRepository = userRepository;
        _perRepository = perRepository;
        _logger = _logger;
    }

   public async Task<UserPerModel?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
{
    var user = await _userRepository.GetByIdAsync(id, cancellationToken);
    if (user == null) return null;

    // Validar la existencia del ObjectId en MongoDB
    var personaEntity = await _perRepository.GetByIdAsync(user.Persona, cancellationToken);
    if (personaEntity == null)
    {
        throw new KeyNotFoundException($"Persona con ObjectId {user.Persona} no encontrada.");
    }

    return new UserPerModel
    {
        Id = user.Id,
        Name = user.Name,
        Persona = user.Persona // Almacenar el ObjectId del registro en MongoDB
    };
}

   public async Task<IEnumerable<UserPerModel>> GetUserByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
{
    var users = await _userRepository.GetByName(name, pageIndex, pageSize, orderBy, cancellationToken);

    var userPerModels = new List<UserPerModel>();
    foreach (var user in users)
    {
        // Validar la existencia del ObjectId en MongoDB
        var personaEntity = await _perRepository.GetByIdAsync(user.Persona, cancellationToken);
        if (personaEntity == null)
        {
            throw new KeyNotFoundException($"Persona con ObjectId {user.Persona} no encontrada.");
        }

        userPerModels.Add(new UserPerModel
        {
            Id = user.Id,
            Name = user.Name,
            Persona = user.Persona // Almacenar el ObjectId del registro en MongoDB
        });
    }

    return userPerModels;
}

   public async Task<UserPerModel> CreateUserAsync(string name, string personaId, CancellationToken cancellationToken)
{
    try
    {
        // Validar el ID de la persona en MongoDB
        var persona = await _perRepository.GetByIdAsync(personaId, cancellationToken);
        if (persona == null)
        {
            throw new ArgumentException($"Persona ID {personaId} no encontrada.");
        }

        // Crear el usuario en la base SQL
        var user = await _userRepository.CreateAsync(name, personaId, cancellationToken);

        // Retornar el modelo combinado
        return new UserPerModel
        {
            Id = user.Id,
            Name = user.Name,
            Persona = personaId // Almacenar el ObjectId del registro en MongoDB
        };
    }
    catch (Exception ex)
    {
        // Loggear la excepción detallada
        _logger.LogError(ex, "An error occurred while creating the user.");
        throw; // Rethrow la excepción para que pueda ser manejada en niveles superiores si es necesario
    }
}

    public async Task UpdateUserAsync(int id, string name, string personaId, CancellationToken cancellationToken)
{
    // Validar existencia del usuario
    var user = await _userRepository.GetByIdAsync(id, cancellationToken);
    if (user == null) throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

    // Validar la persona en MongoDB
    var persona = await _perRepository.GetByIdAsync(personaId, cancellationToken);
    if (persona == null)
    {
        throw new ArgumentException($"Persona ID {personaId} no encontrada.");
    }

    // Actualizar el usuario
    await _userRepository.UpdateUserAsync(id, name, personaId, cancellationToken);
}

    public async Task DeleteUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        // Validar existencia del usuario
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null) throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        // Eliminar el usuario
        await _userRepository.DeleteByIdAsync(id, cancellationToken);
    }

   public async Task<UserPerModel> PatchUserAsync(int id, PatchUserRequest patchUser, CancellationToken cancellationToken)
{
    // Obtener el usuario desde la base de datos SQL.
    var user = await _userRepository.GetByIdAsync(id, cancellationToken);
    if (user == null)
    {
        return null; // Retornar null si el usuario no existe.
    }

    // Actualizar el nombre si está presente en el request.
    if (!string.IsNullOrEmpty(patchUser.Name))
    {
        user.Name = patchUser.Name;
    }

    // Actualizar el ObjectId de Persona si se proporciona.
    if (!string.IsNullOrEmpty(patchUser.Persona))
    {
        // Verificar que la Persona existe en la base de datos MongoDB (SOAP API).
        var persona = await _perRepository.GetByIdAsync(patchUser.Persona, cancellationToken);
        if (persona != null)
        {
            user.Persona = patchUser.Persona; // Agregar solo si existe.
        }
    }

    // Actualizar el usuario en la base de datos SQL.
    await _userRepository.UpdateUserAsync(user.Id, user.Name, user.Persona, cancellationToken);

    // Retornar el usuario actualizado.
    return new UserPerModel
    {
        Id = user.Id,
        Name = user.Name,
        Persona = user.Persona // Almacenar el ObjectId del registro en MongoDB
    };
}

}
