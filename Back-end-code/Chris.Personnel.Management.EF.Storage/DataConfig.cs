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
            return "ChrisPersonnelManagementSQlServerDB";
            //return "ChrisPersonnelManagementMySqlDB";
        }

        public string GetConnectionString()
        {
            var connectionStringKey = GetConnectionStringKey();

            return AppSettings.Apply("ConnectionStrings", connectionStringKey);
        }
    }
}
