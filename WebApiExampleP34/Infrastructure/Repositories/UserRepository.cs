using WebApiExampleP34.Application.Repositories;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DAL;

namespace WebApiExampleP34.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TodoListContext context): base(context)
    {

    }
}
