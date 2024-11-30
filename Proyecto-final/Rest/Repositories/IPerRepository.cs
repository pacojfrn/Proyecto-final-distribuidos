using Rest.Infraestructure.Entities.PerEntity;

namespace Rest.Repositories
{
    public interface IPerRepository
    {
        Task<PerEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
        
    }
}
