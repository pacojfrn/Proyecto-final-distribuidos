using System.ServiceModel;
using Soap.Dtos;

namespace Soap.Contracts;

[ServiceContract]

public interface IPerContract{
    [OperationContract]
    public Task<PerResponseDto> GetById(string id, CancellationToken cancellationToken);
    [OperationContract]
    public Task<IList<PerResponseDto>> GetAll(CancellationToken cancellationToken);
    [OperationContract]
    public Task<PerResponseDto> GetByName(string name, CancellationToken cancellationToken);
    [OperationContract]
    public Task<bool> DeleteById(string id, CancellationToken cancellationToken);
    [OperationContract]
    public Task<IList<PerResponseDto>> GetByArcana(string arcana, CancellationToken cancellationToken);
}
