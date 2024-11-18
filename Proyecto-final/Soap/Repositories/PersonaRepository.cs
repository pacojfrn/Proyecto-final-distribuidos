using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SoapApi.Infrastructure;
using SoapApi.Mappers;
using SoapApi.Models;

namespace Soap.Repositories;

public class PersonaRespository : IPersonaRepository{

    private readonly IMongoCollection<GroupEntity> _personasCollection;
    public GroupRepository(IMongoClient mongoClient, IConfiguration configuration){
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Groups:DatabaseName"));
        _groups = database.GetCollection<GroupEntity>(configuration.GetValue<string>("MongoDb:Groups:CollectionName"));
    }

    public async Task<IList<PersonaResponseDto>> GetAll(int page, int limit, CancellationToken cancellationToken)
        {
            var personas = await _personasCollection.Find(persona => true).Skip((page - 1) * limit).Limit(limit).ToListAsync(cancellationToken);
            var totalPersonas = await _personasCollection.CountDocumentsAsync(persona => true, cancellationToken: cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalPersonas / limit);

            var result = new
            {
                totalPersonas,
                totalPages,
                currentPage = page,
                personas
            };

            // Guardar en caché (30 segundos)
                await _cache.SetStringAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

                return Ok(result);
            }
        }

        public async Task<PersonaResponseDto> GetByName(string name, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByNameAsync(name, cancellationToken);
            if (persona is not null)
            {
                return persona.ToDto();
            }
            throw new FaultException("Persona not found");
        }

        public async Task<PersonaResponseDto> GetById(string id, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id, cancellationToken);
            if (persona is not null){
                return persona.ToDto();
            }
            throw new FaultException("Persona not found");
        }

        public async Task<IList<PersonaResponseDto>> GetByArcana(string arcana, int page, int limit, CancellationToken cancellationToken)
        {
            var personas = await _personaRepository.GetPersonaByArcana(arcana,page,limit, cancellationToken);
            return personas.Select(persona => persona.ToDto()).ToList();
        }

        public async Task<PersonaResponseDto> CreatePersona(PersonaCreateRequestDto personaRequest, CancellationToken cancellationToken)
        {
            var persona = personaRequest.ToModel();
            var createdPersona = await _personaRepository.CreateAsync(persona,cancellationToken);
            return createdPersona.ToDto();
        }

        public async Task<PersonaResponseDto> UpdatePersonaPartially(string id, PersonaUpdateDto updates, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id, cancellationToken);
            if (persona is null)
            {
                throw new FaultException("Persona not found");
            }

            var updatedPersona = await _personaRepository.UpdateAsync(persona, cancellationToken);
            return updatedPersona.ToDto();
        }

        public async Task<PersonaResponseDto> ReplacePersona(PersonaUpdateDto personaRequest, CancellationToken cancellationToken)
        {
            var persona = personaRequest.ToModel();
            await _personaRepository.GetByIdAsync(persona.id, cancellationToken);
            if (persona is not null){
                return await _personaRepository.UpdatePersona(persona, cancellationToken);
            }
            
                throw new FaultException("Persona not found");
        }

        public async Task<bool> DeletePersonaById(string id, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id,cancellationToken);
            if (persona is null)
            {
                throw new FaultException("Persona not found");
            }
            await _personaRepository.DeleteByIdAsync(persona, cancellationToken);
            return true;
        }
}