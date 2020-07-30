using Chris.Personnel.Management.Common.Helper;
using Microsoft.EntityFrameworkCore.Design;

namespace Chris.Personnel.Management.EF.Storage
{
    /// <summary>
    /// 生成SqlServer数据库必须有此class
    /// </summary>
    public class DesignTimeSqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
    {
        private readonly AppSettings _appSettings;

        /// <summary>
        /// 不可删除，否则migration 报错
        /// </summary>
        public DesignTimeSqlServerContextFactory()
        {
            
        }

        public DesignTimeSqlServerContextFactory(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public SqlServerContext CreateDbContext(string[] args)
        {
            return new SqlServerContext(new ConnectionStringManager(_appSettings));
        }
    }
}