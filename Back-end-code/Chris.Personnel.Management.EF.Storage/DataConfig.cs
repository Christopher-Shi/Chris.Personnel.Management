using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;

namespace Chris.Personnel.Management.EF.Storage
{
    public class DataConfig
    {
        public AppSettings AppSettings { get; }

        public DataConfig(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }

        public string GetConnectionString()
        {
            var connectionString = default(string);

            // SQlServer
            if (AppSettings.Apply("ConnectionStrings", "SQlServerDB", "Enabled").ToBool())
            {
                connectionString = AppSettings.Apply("ConnectionStrings", "SQlServerDB", "ConnectionString");
            }
            // MySqlDB
            if (AppSettings.Apply("ConnectionStrings", "MySqlDB", "Enabled").ToBool())
            {
                connectionString = AppSettings.Apply("ConnectionStrings", "MySqlDB", "ConnectionString");
            }
            // Sqlite
            if (AppSettings.Apply("ConnectionStrings", "SqliteDB", "Enabled").ToBool())
            {
                connectionString = AppSettings.Apply("ConnectionStrings", "SqliteDB", "ConnectionString");
            }

            return connectionString;
        }
    }
}
