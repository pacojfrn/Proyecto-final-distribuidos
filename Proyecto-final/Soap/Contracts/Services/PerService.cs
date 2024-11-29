using System.ServiceModel;
using Soap.Contracts;
using Soap.Dtos;
using Soap.Mappers;
using Soap.Repositories;

namespace Soap.Services;

public class PerService : IPerContract{


    private readonly IPerRepository _perRepository;

    public PerService(IPerRepository perRepository){
        _perRepository = perRepository;
    }

    public async Task<IList<PerResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var personas = await _perRepository.GetAllAsync(cancellationToken);
            // Mapeamos cada PersonaEntity a PerResponseDto
            return personas.Select(persona => persona.ToModel().ToDto()).ToList();
        }

    public async Task<IList<PerResponseDto>> GetByArcana(string arcana, CancellationToken cancellationToken)
    {
        var personas = await _perRepository.GetByArcanaAsync(arcana, cancellationToken);
        return personas.Select(persona => persona.ToDto()).ToList();
    }

    public async Task<PerResponseDto> GetById(string id, CancellationToken cancellationToken){
        var persona = await _perRepository.GetByIdAsync(id, cancellationToken);
        if (persona is not null){
            return persona.ToDto();
        }
        throw new FaultException("Persona not found");
    }
    public async Task<PerResponseDto> GetByName(string name, CancellationToken cancellationToken){
        var persona = await _perRepository.GetByNameAsync(name, cancellationToken);
        if (persona is not null){
            return persona.ToDto();
        }
        throw new FaultException("Persona not found");
    }
    public async Task<bool> DeleteById(string id, CancellationToken cancellationToken){
        var delete = await _perRepository.DeleteByIdAsync(id, cancellationToken);
        if (delete){
            return true;
        }
        throw new FaultException("Persona not found");
    }
    public async Task<PerResponseDto> CreatePersona(PerResponseDto persona, CancellationToken cancellationToken){
        var createdPer = persona.toEntity(); 
        var result = await _perRepository.CreatePersonaAsync(createdPer, cancellationToken);
        if (result is not null){
            return result.ToDto();
        }
        throw new FaultException("Persona not created");
    }
}