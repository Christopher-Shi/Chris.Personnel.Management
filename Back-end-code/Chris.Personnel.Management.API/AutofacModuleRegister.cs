using Autofac;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.LogicService;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.Repository;

namespace Chris.Personnel.Management.API
{
    internal class AutofacModuleRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            QueryServiceInstaller.ConfigureContainer(builder);

            LogicServiceInstaller.ConfigureContainer(builder);

            RepositoryInstaller.ConfigureContainerForSqlServer(builder);

            CommonInstaller.ConfigureContainer(builder);
        }
    }
}
