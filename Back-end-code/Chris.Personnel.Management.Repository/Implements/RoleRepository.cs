using Chris.Personnel.Management.Entity;

namespace Chris.Personnel.Management.Repository.Implements
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbContextProvider dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
