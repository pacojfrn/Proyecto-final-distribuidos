using MongoDB.Driver;
using MongoDB.Bson;
using Soap.Infrastructure.Entities;

namespace Soap.Repositories
{
    public class PerRepository : IPerRepository
    {
        private readonly IMongoCollection<PerEntity> _personas;

        public PerRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Persona:DatabaseName"));
            _personas = database.GetCollection<PerEntity>(configuration.GetValue<string>("MongoDb:Persona:CollectionName"));
        }

        public async Task<IList<PerEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _personas.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<IList<PerEntity>> GetByArcanaAsync(string arcana, CancellationToken cancellationToken)
        {
            return await _personas.Find(p => p.arcana == arcana).ToListAsync(cancellationToken);
        }

        public async Task<PerEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format", nameof(id));
            }

            return await _personas.Find(p => p.id == objectId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<PerEntity> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _personas.Find(p => p.name == name).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteByNameAsync(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }

            var registro = GetByNameAsync(name, cancellationToken);
            if (registro is not null){
                await _personas.DeleteOneAsync(p => p.name == name, cancellationToken);
                return true;
            }
            return false;
        }

        public async Task<PerEntity> CreatePersonaAsync(PerEntity persona, CancellationToken cancellationToken){
            await _personas.InsertOneAsync(persona, new InsertOneOptions(), cancellationToken);
            return persona;
        }
    }
}
