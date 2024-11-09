using Microsoft.EntityFrameworkCore;
using SoapApi.Infrastructure.Entities;

namespace SoapApi.Infrastructure;

public class RelationalDbContext : DbContext{
    public RelationalDbContext(DbContextOptions<RelationalDbContext> options) : base(options){

    }

    public DbSet<UserEntity> Users{ get; set; }
<<<<<<< HEAD
    public DbSet<BookEntity> Books{ get; set; }
=======
<<<<<<< HEAD
    public DbSet<BookEntity> Books{ get; set; }
=======
<<<<<<< HEAD
    public DbSet<BookEntity> Books{ get; set; }
=======
>>>>>>> main
>>>>>>> 64852ac540bb66c270558a06e08e6f7cd8e9920e
>>>>>>> d65eae242f824823ad62e338375cdadfff41386a
    

}

