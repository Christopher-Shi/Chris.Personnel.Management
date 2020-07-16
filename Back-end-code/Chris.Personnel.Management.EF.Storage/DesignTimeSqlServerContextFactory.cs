using Chris.Personnel.Management.Common.Helper;
using Microsoft.EntityFrameworkCore.Design;

namespace Chris.Personnel.Management.EF.Storage
{
    /// <summary>
    /// 生成SqlServer数据库必须有此class
    /// </summary>
    public class DesignTimeSqlServerContextFactory : IDesignTimeDbContextFactory<SqlServerContext>
    {
        private readonly Appsettings _appsettings;

        /// <summary>
        /// 不可删除，否则migration 报错
        /// </summary>
        public DesignTimeSqlServerContextFactory()
        {
            
        }

        public DesignTimeSqlServerContextFactory(Appsettings appsettings)
        {
            _appsettings = appsettings;
        }

        public SqlServerContext CreateDbContext(string[] args)
        {
            return new SqlServerContext(new ConnectionStringManager(_appsettings));
        }
    }
}