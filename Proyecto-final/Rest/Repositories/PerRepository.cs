using MongoDB.Driver;
using MongoDB.Bson;
using Rest.Infraestructure.Entities.PerEntity;

namespace Rest.Repositories
{
    public class PerRepository : IPerRepository
    {
        private readonly IMongoCollection<PerEntity> _personas;

        public PerRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(configuration.GetValue<string>("MongoDb:Persona:DatabaseName"));
            _personas = database.GetCollection<PerEntity>(configuration.GetValue<string>("MongoDb:Persona:CollectionName"));
        }

        public async Task<PerEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException("Invalid ID format", nameof(id));
            }

            return await _personas.Find(p => p.id == objectId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}

       