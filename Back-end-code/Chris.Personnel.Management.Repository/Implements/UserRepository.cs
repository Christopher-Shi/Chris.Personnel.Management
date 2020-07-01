using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.Repository.Implements
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
