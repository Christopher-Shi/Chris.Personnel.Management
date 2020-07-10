using Chris.Personnel.Management.Common;

namespace Chris.Personnel.Management.EF.Storage
{
    public class DataConfig
    {
        public Appsettings Appsettings { get; }

        public DataConfig(Appsettings appsettings)
        {
            Appsettings = appsettings;
        }
        private string GetConnectionStringKey()
        {
            return "ChrisPersonnelManagementDB";
        }

        public string GetConnectionString()
        {
            var connectionStringKey = GetConnectionStringKey();

            return Appsettings.Apply("ConnectionStrings", connectionStringKey);
        }
    }
}
