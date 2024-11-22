using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rest.Dtos;
using Rest.Mappers;
using Rest.Services;
using Rest.Cache;
using Rest.Infraestructure;
using Newtonsoft.Json;

namespace Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase{
    private readonly IUserService _userService;
    private readonly IRedisCacheService _cacheService;

    public UserController(IUserService userService, IRedisCacheService cacheService){
        _userService = userService;
        _cacheService = cacheService;
    }

    [HttpGet("GetById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> GetUserById(int id, CancellationToken cancellationToken){
        string cacheKey = $"User:{id}";
        var cachedUser = await _cacheService.GetCacheValueAsync(cacheKey);
        if(cachedUser != null){
            return Ok(JsonConvert.DeserializeObject<UserResponse>(cachedUser));
        }

        var user = await _userService.GetUserByIdAsync(id, cancellationToken);
        if (user is null){
            return NotFound();
        }
        var userDto = user.ToDto();
        await _cacheService.SetCacheValueAsync(cacheKey, JsonConvert.SerializeObject(userDto), TimeSpan.FromMinutes(5));
        return Ok(userDto);
    }

    [HttpGet("GetByName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUserByName(
        CancellationToken cancellationToken,
        [FromQuery] string name, 
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "name"){
            string cacheKey = $"Users:{name}:{pageIndex}:{pageSize}:{orderBy}";
            var cachedUsers = await _cacheService.GetCacheValueAsync(cacheKey);

            if(cachedUsers != null){
                return Ok(JsonConvert.DeserializeObject<List<UserResponse>>(cachedUsers));
            }

            var users = await _userService.GetUserByNameAsync(name, pageIndex, pageSize, orderBy, cancellationToken);
            if(users == null || !users.Any()){
                return Ok(new List<UserResponse>());
            }
            var userDto = users.Select(user => user.ToDto()).ToList();
            await _cacheService.SetCacheValueAsync(cacheKey, JsonConvert.SerializeObject(userDto), TimeSpan.FromMinutes(5));
            return Ok(userDto);
    }

    [HttpDelete("DeleteById{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUser(int id, CancellationToken cancellationToken){
        try{
            await _userService.DeleteUserByIdAsync(id, cancellationToken);
            return NoContent();
        }catch(Exception){
            return NotFound();
        }
    }

    [HttpPost("CreateUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest userRequest, CancellationToken cancellationToken){
        try{
            var user = await _userService.CreateUserAsync(userRequest.Name, userRequest.Persona, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id}, user.ToDto());
        }catch(Exception){
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the user");
        }
    }

    [HttpPut("PutById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest userUpdate, CancellationToken cancellationToken){
        try{
            await _userService.UpdateUserAsync(id, userUpdate.Name, userUpdate.Persona, cancellationToken);
            return NoContent();
        }catch(Exception){
            return NotFound();
        }
    }

    [HttpPatch("PatchById{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PatchUser(int id, [FromBody] PatchUserRequest patchuser, CancellationToken cancellationToken){
        try{
            var userupdated = await _userService.PatchUserAsync(id, patchuser, cancellationToken);
            if(userupdated == null){
                return NotFound();
            }
            return Ok(userupdated.ToDto());
        }catch (Exception ex){
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        }
    }

}	