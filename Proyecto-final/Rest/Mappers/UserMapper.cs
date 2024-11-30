using Rest.Dtos;
using Rest.Infraestructure;
using Rest.Infraestructure.Entities;
using Rest.Models;

namespace Rest.Mappers;

public static class UserMapper{

    public static UserResponse ToDto(this UserModel user){
        return new UserResponse{
            Id = user.Id,
            Name = user.Name,
            Persona = user.Persona
        };
    }

    public static UserModel ToModel(this UserEntity user){
        if (user is null){
            return null;
        }

        return new UserModel{
            Id = user.Id,
            Name = user.Name,
            Persona = user.Persona
        };
    }

    public static CreateUserRequest ToModel(this UserModel user){
        return new CreateUserRequest{
            Name = user.Name,
            Persona = user.Persona
        };
    }

    public static UserModel ToModel(this CreateUserRequest request){
        return new UserModel{
            Name = request.Name,
            Persona = request.Persona
        };
    }

    public static UserResponse ToDto(this UserPerModel userModel){
        return new UserResponse{
            Id = userModel.Id,
            Name = userModel.Name,
            Persona = userModel.Persona
        };
    }
}
