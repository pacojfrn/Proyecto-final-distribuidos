using Soap.Infrastructure.Entities;

namespace Soap.Repositories
{
    public interface IPerRepository
    {
        Task<IList<PerEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<IList<PerEntity>> GetByArcanaAsync(string arcana, CancellationToken cancellationToken);
        Task<PerEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<PerEntity> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken);
    }
}
