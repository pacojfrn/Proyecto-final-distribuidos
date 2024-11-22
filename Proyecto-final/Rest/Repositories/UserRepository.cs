using Microsoft.EntityFrameworkCore;
using Rest.Infraestructure;
using Rest.Infraestructure.Entities;
using Rest.Models;
using Rest.Mappers;

namespace Rest.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<UserModel> CreateAsync(string name, List<string> persona, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity
            {
                // El Id se gestionará automáticamente
                Name = name,
                Persona = persona
            };

            await _dbcontext.Users.AddAsync(userEntity, cancellationToken);
            await _dbcontext.SaveChangesAsync(cancellationToken);

            return userEntity.ToModel();
        }

        public async Task DeleteByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var userEntity = await _dbcontext.Users.FindAsync(new object[] { Id }, cancellationToken);
            if (userEntity != null)
            {
                _dbcontext.Users.Remove(userEntity);
                await _dbcontext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<UserModel> GetByIdAsync(int UserId, CancellationToken cancellationToken)
        {
            var user = await _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == UserId, cancellationToken);
            return user.ToModel();
        }

        public async Task UpdateUserAsync(int id, string name, List<string> persona, CancellationToken cancellationToken)
        {
            var user = await _dbcontext.Users.FindAsync(new object[] { id }, cancellationToken);

            if (user == null)
            {
                return; // No hacer nada si el usuario no existe
            }

            user.Name = name;
            user.Persona = persona;

            _dbcontext.Users.Update(user);
            try
            {
                await _dbcontext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                // En caso de error, solo retorna Task.CompletedTask
                return;
            }

            // No necesitas retornar nada en un método async de tipo Task
        }

        public async Task<IEnumerable<UserModel>> GetByName(string name, int pageIndex, int pageSize, string orderBy, CancellationToken cancellationToken){
            if (string.IsNullOrEmpty(name)){
                return Enumerable.Empty<UserModel>();}

            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Max(1, pageSize);

            var query = _dbcontext.Users.AsNoTracking().Where(u => u.Name.Contains(name));

            query = orderBy.ToLower() switch{
                "name" => query.OrderBy(u => u.Name),
                "id" => query.OrderBy(u => u.Id),
                _ => query
            };

            var users = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

            return users.Select(u => u.ToModel());
        }

    }
}
