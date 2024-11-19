using MongoDB.Bson;
using MongoDB.Driver;
using Soap.Mappers;
using Soap.Models;
using Soap.Infrastructure.Entity;
using Soap.Contracts.Dtos;

namespace Soap.Repositories;

public class PersonaRespository : IPersonaRepository{

    private readonly IMongoCollection<PersonaEntity> _personasCollection;
    public PersonaRepository(IMongoClient mongoClient, IConfiguration configuration){
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Groups:DatabaseName"));
        _personasCollection = database.GetCollection<PersonaEntity>(configuration.GetValue<string>("MongoDb:Groups:CollectionName"));
    }

    public async Task<IList<PersonaModel>> GetAll(int page, int limit, CancellationToken cancellationToken)
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

            return result;
        }
        

        public async Task<PersonaModel> GetByName(string name, CancellationToken cancellationToken)
        {
            var filter = Builders<PersonaEntity>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i")); // Búsqueda por coincidencia parcial
            var persona = await _personasCollection.Find(filter);
            if (persona is not null)
            {
                return persona.ToModel();
            }
            throw new FaultException("Persona not found");
        }

        public async Task<PersonaModel> GetById(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<PersonaEntity>.Filter.Eq(x => x.id, id);
            var persona = await _personasCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);            
            if (persona is not null){
                return persona.ToModel();
            }
            throw new FaultException("Persona not found");
        }

        public async Task<IList<PersonaModel>> GetByArcana(string arcana, int page, int limit, CancellationToken cancellationToken)
        {
            var filter = Builders<PersonaEntity>.Filter.Eq(x => x.Arcana, arcana);
            var personas = await _personasCollection.Find(filter).Skip((page - 1) * limit).Limit(limit).ToListAsync(cancellationToken);
            var totalPersonas = await _personasCollection.CountDocumentsAsync(persona => true, cancellationToken: cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalPersonas / limit);

            var result = new
            {
                totalPersonas,
                totalPages,
                currentPage = page,
                personas
            };

                return result;
        }

        public async Task<PersonaCreateRequestDto> CreatePersona(PersonaCreateRequestDto personaRequest, CancellationToken cancellationToken)
        {
            var persona = new PersonaEntity{
                Arcana = personaRequest.Arcana,
                Weak = personaRequest.Weak,
                Stats = personaRequest.Stats,
                Strength = personaRequest.Strength,
                Level = personaRequest.Level,
                Name = personaRequest.Name
            };
            await _personasCollection.InsertOneAsync(persona, new InsertOneOptions(), cancellationToken);
            return persona;
        }

        public async Task<PersonaModel> UpdatePersonaPartially(string id, PersonaUpdateDto updates, CancellationToken cancellationToken)
        {
            // Buscar la Persona por ID
            var persona = await _personaRepository.GetByIdAsync(id, cancellationToken);
            if (persona is null)
            {
                throw new FaultException("Persona not found");
            }

            // Aplicar cambios parciales (asumiendo que `PersonaUpdateDto` contiene solo los campos actualizables)
            persona.Arcana = updates.Arcana ?? persona.Arcana;
            persona.Weak = updates.Weak ?? persona.Weak;
            persona.Stats = updates.Stats ?? persona.Stats;
            persona.Strength = updates.Strength ?? persona.Strength;
            persona.Level = updates.Level != default ? updates.Level : persona.Level;
            persona.Name = updates.Name ?? persona.Name;

            // Guardar los cambios
            var updatedPersona = await _personaRepository.UpdateAsync(persona, cancellationToken);
            return updatedPersona.ToDto();
        }

        public async Task<PersonaModel> ReplacePersona(PersonaUpdateDto personaRequest, CancellationToken cancellationToken)
        {
            // Convertir PersonaUpdateDto a PersonaEntity
            var persona = personaRequest.ToModel();
            
            // Buscar Persona por ID (asumiendo que `personaRequest` tiene un campo de ID)
            var existingPersona = await _personaRepository.GetByIdAsync(persona.Id, cancellationToken);
            
            if (existingPersona is not null)
            {
                // Reemplazo total
                var updatedPersona = await _personaRepository.UpdatePersona(persona, cancellationToken);
                return updatedPersona.ToDto();
            }

            throw new FaultException("Persona not found");
        }

        public async Task<bool> DeletePersonaById(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<GroupEntity>.Filter.Eq(s => s.id,id);
            await _personasCollection.DeleteOneAsync(filter,cancellationToken);
            return true;
        }
}