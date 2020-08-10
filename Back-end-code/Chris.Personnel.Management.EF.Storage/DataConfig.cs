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
        private string GetConnectionStringKey()
        {
            //return "ChrisPersonnelManagementSQlServerDB"; //SQlServer
            //return "ChrisPersonnelManagementMySqlDB"; //MySqlDB
            return "ChrisPersonnelManagementSqliteDb"; //Sqlite
        }

        public string GetConnectionString()
        {
            var connectionStringKey = GetConnectionStringKey();

            return AppSettings.Apply("ConnectionStrings", connectionStringKey);
        }
    }
}
