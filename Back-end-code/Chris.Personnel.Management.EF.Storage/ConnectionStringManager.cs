using Chris.Personnel.Management.Common;
using Microsoft.Extensions.Configuration;

namespace Chris.Personnel.Management.EF.Storage
{
    public class ConnectionStringManager : IConnectionStringManager
    {
        public ConnectionStringManager(Appsettings appsettings)
        {
            ConnectionString = new DataConfig(appsettings).GetConnectionString();
        }

        public string ConnectionString { get; }
    }
}
