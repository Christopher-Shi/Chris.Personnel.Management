using Chris.Personnel.Management.Common.Helper;

namespace Chris.Personnel.Management.EF.Storage
{
    public class ConnectionStringManager : IConnectionStringManager
    {
        public ConnectionStringManager(AppSettings appSettings)
        {
            ConnectionString = new DataConfig(appSettings).GetConnectionString();
        }

        public string ConnectionString { get; }
    }
}
