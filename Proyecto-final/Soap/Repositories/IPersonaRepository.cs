using Soap.Models;
using Soap.Contracts.Dtos;

namespace Soap.Repositories;

public interface IPersonaRepository {
    Task<IList<PersonaModel>> GetAll(int page, int limit,CancellationToken cancellationToken);
    public Task<PersonaModel> GetByName(string name, CancellationToken cancellationToken);
    public Task<PersonaModel> GetById(string id, CancellationToken cancellationToken);
    public Task<IList<PersonaModel>> GetByArcana(string arcana, int page, int limit, CancellationToken cancellationToken);
    public Task<PersonaCreateRequestDto> CreatePersona(PersonaCreateRequestDto personaRequest, CancellationToken cancellationToken);
    public Task<PersonaModel> UpdatePersonaPartially(string id, PersonaUpdateDto updates, CancellationToken cancellationToken);
    public Task<PersonaModel> ReplacePersona(PersonaUpdateDto personaRequest, CancellationToken cancellationToken);
    public Task<bool> DeletePersonaById(string id, CancellationToken cancellationToken);

}


