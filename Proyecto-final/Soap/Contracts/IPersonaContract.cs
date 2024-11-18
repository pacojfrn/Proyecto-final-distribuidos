using SoapApi.Models;

namespace Soap.Contracts
public interface IPersonaContract
{
    Task<PersonaResponseDto> GetById (string Id, CancellationToken cancellationToken);

    Task<IList<PersonaResponseDto>> GetAll (CancellationToken cancellationToken);

    Task<IList<PersonaResponseDto>> GetByArcana(string arcana, int page, int limit, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken); 

    Task <bool> DeletePersonaById(string id, CancellationToken cancellationToken);

    Task<PersonaResponseDto> CreatePersona(PersonaCreateRequestDto personaRequest, CancellationToken cancellationToken);

    Task<PersonaResponseDto> GetByName(string name, CancellationToken cancellationToken);

    Task <PersonaResponseDto> UpdatePersonaPartially(string id, PersonaUpdateDto updates, CancellationToken cancellationToken);

    Task <PersonaResponseDto> ReplacePersona(PersonaUpdateDto personaRequest, CancellationToken cancellationToken);
}