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

    public UserService(IUserRepository userRepository, IPerRepository perRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _perRepository = perRepository;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

  public async Task<UserPerModel?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
{
    try
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Usuario con ID {UserId} no encontrado.", id);
            throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
        }

        // Validar la existencia del ObjectId en MongoDB
        _logger.LogInformation("Validando Persona ObjectId: {ObjectId}", user.Persona);
        var personaEntity = await _perRepository.GetByIdAsync(user.Persona, cancellationToken);
        if (personaEntity == null)
        {
            _logger.LogWarning("Persona con ObjectId {ObjectId} no encontrada.", user.Persona);
            throw new KeyNotFoundException($"Persona con ObjectId {user.Persona} no encontrada.");
        }

        return new UserPerModel
        {
            Id = user.Id,
            Name = user.Name,
            Persona = user.Persona
        };
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener el usuario por ID.");
        throw;
    }
}

  public async Task<IEnumerable<UserPerModel>> GetUserByNameAsync(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken)
{
    try
    {
        var users = await _userRepository.GetByName(name, pageIndex, pageSize, orderBy, cancellationToken);

        var userPerModels = new List<UserPerModel>();
        foreach (var user in users)
        {
            _logger.LogInformation("Validando Persona ObjectId: {ObjectId}", user.Persona);
            var personaEntity = await _perRepository.GetByIdAsync(user.Persona, cancellationToken);
            if (personaEntity == null)
            {
                _logger.LogWarning("Persona con ObjectId {ObjectId} no encontrada.", user.Persona);
                throw new KeyNotFoundException($"Persona con ObjectId {user.Persona} no encontrada.");
            }

            userPerModels.Add(new UserPerModel
            {
                Id = user.Id,
                Name = user.Name,
                Persona = user.Persona
            });
        }

        return userPerModels;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener usuarios por nombre.");
        throw;
    }
}


public async Task<UserPerModel> CreateUserAsync(string name, string personaId, CancellationToken cancellationToken)
{
    try
    {
        // Log para la validación del ID de la persona en MongoDB
        _logger.LogInformation("Validando Persona ObjectId: {ObjectId}", personaId);
        var persona = await _perRepository.GetByIdAsync(personaId, cancellationToken);
        if (persona == null)
        {
            _logger.LogWarning("Persona con ObjectId {ObjectId} no encontrada.", personaId);
            throw new ArgumentException($"Persona con ObjectId {personaId} no encontrada.", nameof(personaId));
        }

        // Log para la creación del usuario en la base SQL
        _logger.LogInformation("Creando usuario con Name: {Name} y Persona ObjectId: {ObjectId}", name, personaId);
        var user = await _userRepository.CreateAsync(name, personaId, cancellationToken);

        // Log para el éxito de la creación del usuario
        _logger.LogInformation("Usuario creado exitosamente con ID: {UserId}", user.Id);

        // Retornar el modelo combinado
        return new UserPerModel
        {
            Id = user.Id,
            Name = user.Name,
            Persona = personaId // Almacenar el ObjectId del registro en MongoDB
        };
    }
    catch (ArgumentException ex)
    {
        // Log y rethrow con mensaje de error específico
        _logger.LogWarning(ex, ex.Message);
        throw new ArgumentException("La persona proporcionada es inválida. Por favor, verifica el ObjectId.", nameof(personaId));
    }
    catch (Exception ex)
    {
        // Log para cualquier otra excepción que ocurra
        _logger.LogError(ex, "Error al crear el usuario.");
        throw;
    }
}

    public async Task DeleteUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        // Validar existencia del usuario
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null) throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");

        // Eliminar el usuario
        await _userRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public async Task UpdateUserAsync(int id, string name, string personaId, CancellationToken cancellationToken) { 
        try { 
            var user = await _userRepository.GetByIdAsync(id, cancellationToken); 
            if (user == null) { 
                _logger.LogWarning("Usuario con ID {UserId} no encontrado.", id); 
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado."); 
            } 
            _logger.LogInformation("Validando Persona ObjectId: {ObjectId}", personaId); 
            var personaEntity = await _perRepository.GetByIdAsync(personaId, cancellationToken); 
            if (personaEntity == null) { 
                _logger.LogWarning("Persona con ObjectId {ObjectId} no encontrada.", personaId); 
                throw new ArgumentException($"Persona con ObjectId {personaId} no encontrada."); 
            } 
            _logger.LogInformation("Actualizando usuario con ID: {UserId}", id); 
            await _userRepository.UpdateUserAsync(id, name, personaId, cancellationToken); 
            _logger.LogInformation("Usuario con ID: {UserId} actualizado correctamente", id); 
        } catch (Exception ex) { 
            _logger.LogError(ex, "Error al actualizar el usuario con ID: {UserId}", id); 
            throw;
        }
    }

   public async Task<UserPerModel> PatchUserAsync(int id, PatchUserRequest patchUser, CancellationToken cancellationToken)
{
    try
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Usuario con ID {UserId} no encontrado.", id);
            return null;
        }

        if (!string.IsNullOrEmpty(patchUser.Name))
        {
            user.Name = patchUser.Name;
        }

        if (!string.IsNullOrEmpty(patchUser.Persona))
        {
            _logger.LogInformation("Validando Persona ObjectId: {ObjectId}", patchUser.Persona);
            var personaEntity = await _perRepository.GetByIdAsync(patchUser.Persona, cancellationToken);
            if (personaEntity == null)
            {
                _logger.LogWarning("Persona con ObjectId {ObjectId} no encontrada.", patchUser.Persona);
                throw new ArgumentException($"Persona con ObjectId {patchUser.Persona} no encontrada.");
            }
            user.Persona = patchUser.Persona;
        }

        await _userRepository.UpdateUserAsync(user.Id, user.Name, user.Persona, cancellationToken);

        return new UserPerModel
        {
            Id = user.Id,
            Name = user.Name,
            Persona = user.Persona
        };
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al actualizar parcialmente el usuario.");
        throw;
    }
}

}
