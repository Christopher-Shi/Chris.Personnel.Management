using Chris.Personnel.Management.Common;
using Microsoft.EntityFrameworkCore.Design;

namespace Chris.Personnel.Management.EF.Storage
{
    /// <summary>
    /// 生成SqlServer数据库必须有此class
    /// </summary>
    public class DesignTimeMySqlContextFactory : IDesignTimeDbContextFactory<MySqlContext>
    {
        private readonly Appsettings _appsettings;

        /// <summary>
        /// 不可删除，否则migration 报错
        /// </summary>
        public DesignTimeMySqlContextFactory()
        {

        }

        public DesignTimeMySqlContextFactory(Appsettings appsettings)
        {
            _appsettings = appsettings;
        }

        public MySqlContext CreateDbContext(string[] args)
        {
            return new MySqlContext(new ConnectionStringManager(_appsettings));
        }
    }
}