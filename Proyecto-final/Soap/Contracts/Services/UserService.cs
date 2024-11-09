using System.ServiceModel;
using System.Xml.Schema;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Mappers;
using SoapApi.Models;
using SoapApi.Repositories;

namespace SoapApi.Services;

public class UserService : IUserContract{


    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository){
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> CreateUser(UserCreateRequestDto userRequest, CancellationToken cancellationToken)
    {
        var user = userRequest.ToModel();
        var createdUser = await _userRepository.CreateAsync(user,cancellationToken);
        return createdUser.ToDto();
    }

    public async Task<bool> DeleteUserById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(userId,cancellationToken);
        if (user is null)
        {
            throw new FaultException("User not found");
        }
        await _userRepository.DeleteByIdAsync(user, cancellationToken);
        return true;
    }

    public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(cancellationToken);
        return users.Select(user => user.ToDto()).ToList();
    }

    public async Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllByEmail(email, cancellationToken);
        return users.Select(user => user.ToDto()).ToList();
    }

    public async Task<UserResponseDto> GetUserById(Guid userId, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is not null){
            return user.ToDto();
        }
        throw new FaultException("User not found");
    }

    public async Task<bool> UpdateUser(UserUpdateDto userUpdate, CancellationToken cancellationToken){
        var user = userUpdate.ToModel();
        await _userRepository.GetByIdAsync(user.Id, cancellationToken);
        if (user is not null){
            return await _userRepository.UpdateUser(user, cancellationToken);
        }
        
            throw new FaultException("User not found");
    }
}

