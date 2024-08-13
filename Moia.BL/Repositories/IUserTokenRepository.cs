using Moia.Shared.Models;

namespace Moia.BL.Repositories
{
    public interface IUserTokenRepository : IRepository<UserToken>
    {
    }

    public class UserTokenRepository : Repository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}