using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rest.Dtos;
using Rest.Mappers;
using Rest.Services;
using Rest.Cache;
using Rest.Models;
using Rest.Infraestructure.Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IRedisCacheService _cacheService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, IRedisCacheService cacheService, ILogger<UserController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("GetById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserPerModel>> GetUserById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            if (user is null)
            {
                _logger.LogWarning("Usuario con ID {UserId} no encontrado.", id);
                return NotFound($"Usuario con ID {id} no encontrado.");
            }
            return Ok(user.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el usuario por ID.");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the user.");
        }
    }

    [HttpGet("GetByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UserPerModel>>> GetUserByName(
        CancellationToken cancellationToken,
        [FromQuery] string name, 
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "name")
    {
        try
        {
            var users = await _userService.GetUserByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);
            if (users == null || !users.Any())
            {
                _logger.LogWarning("Usuarios con nombre {Name} no encontrados.", name);
                return Ok(new List<UserPerModel>());
            }
            return Ok(users.Select(user => user.ToDto()).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios por nombre.");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving users.");
        }
    }

    [HttpDelete("DeleteById/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.DeleteUserByIdAsync(id, cancellationToken);
            _logger.LogInformation("Usuario con ID {UserId} eliminado correctamente.", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Usuario con ID {UserId} no encontrado.", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el usuario con ID {UserId}.", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the user.");
        }
    }

    [HttpPost("CreateUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserPerModel>> CreateUser([FromBody] CreateUserRequest userRequest, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Recibida solicitud para crear usuario con Name: {Name} y Persona ObjectId: {PersonaId}", userRequest.Name, userRequest.Persona);
            var user = await _userService.CreateUserAsync(userRequest.Name, userRequest.Persona, cancellationToken);
            _logger.LogInformation("Usuario creado exitosamente con ID: {UserId}", user.Id);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user.ToDto());
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Solicitud inválida para crear usuario.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el usuario.");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the user.");
        }
    }

    [HttpPut("PutById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest userUpdate, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.UpdateUserAsync(id, userUpdate.Name, userUpdate.Persona, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Solicitud inválida para actualizar usuario.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el usuario con ID {UserId}.", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the user.");
        }
    }

    [HttpPatch("PatchById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PatchUser(int id, [FromBody] PatchUserRequest patchUser, CancellationToken cancellationToken)
    {
        try
        {
            var userUpdated = await _userService.PatchUserAsync(id, patchUser, cancellationToken);
            if (userUpdated == null)
            {
                _logger.LogWarning("Usuario con ID {UserId} no encontrado.", id);
                return NotFound($"Usuario con ID {id} no encontrado.");
            }
            return Ok(userUpdated.ToDto());
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Solicitud inválida para actualizar parcialmente el usuario.");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar parcialmente el usuario con ID {UserId}.", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while partially updating the user.");
        }
    }
}
