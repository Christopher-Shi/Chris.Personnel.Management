using Autofac;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;
using Chris.Personnel.Management.LogicService;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.Repository;

namespace Chris.Personnel.Management.Work
{
    public class AutofacModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            QueryServiceInstaller.ConfigureContainer(builder);

            LogicServiceInstaller.ConfigureContainer(builder);

            // SQlServer
            if (AppSettings.Apply("ConnectionStrings", "SQlServerDB", "Enabled").ToBool())
            {
                RepositoryInstaller.ConfigureContainerForSqlServer(builder);
            }
            // MySqlDB
            if (AppSettings.Apply("ConnectionStrings", "MySqlDB", "Enabled").ToBool())
            {
                RepositoryInstaller.ConfigureContainerForMySql(builder);
            }

            // Sqlite
            if (AppSettings.Apply("ConnectionStrings", "SqliteDb", "Enabled").ToBool())
            {
                RepositoryInstaller.ConfigureContainerForSqllite(builder);
            }

            CommonInstaller.ConfigureContainer(builder);
        }
    }
}
