using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IUserRepository {
    Task<IList<UserModel>> GetAll(CancellationToken cancellationToken);
    Task<IList<UserModel>> GetAllByEmail(string email, CancellationToken cancellationToken);
    public Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken) ;

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> main
>>>>>>> 64852ac540bb66c270558a06e08e6f7cd8e9920e
>>>>>>> d65eae242f824823ad62e338375cdadfff41386a
    public Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken);

    public Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken);

    public Task<bool> UpdateUser(UserModel user, CancellationToken cancellationToken);

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
>>>>>>> f687bda72c2c50207500665b583f0bc2963e378f
>>>>>>> main
>>>>>>> 64852ac540bb66c270558a06e08e6f7cd8e9920e
>>>>>>> d65eae242f824823ad62e338375cdadfff41386a
}


