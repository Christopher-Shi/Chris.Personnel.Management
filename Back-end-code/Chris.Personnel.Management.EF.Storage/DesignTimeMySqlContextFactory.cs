using Chris.Personnel.Management.Common.Helper;
using Microsoft.EntityFrameworkCore.Design;

namespace Chris.Personnel.Management.EF.Storage
{
    /// <summary>
    /// 生成SqlServer数据库必须有此class
    /// </summary>
    public class DesignTimeMySqlContextFactory : IDesignTimeDbContextFactory<MySqlContext>
    {
        private readonly AppSettings _appSettings;

        /// <summary>
        /// 不可删除，否则migration 报错
        /// </summary>
        public DesignTimeMySqlContextFactory()
        {

        }

        public DesignTimeMySqlContextFactory(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public MySqlContext CreateDbContext(string[] args)
        {
            return new MySqlContext(new ConnectionStringManager(_appSettings));
        }
    }
}