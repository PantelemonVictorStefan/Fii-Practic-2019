using System.Configuration;

namespace AppBase.ORM
{
    public class CommonHelpers
    {
        /// <summary>
        /// Get connection string
        /// </summary>
        /// <returns>Connection string</returns>
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["AppBase"]
                .ConnectionString;
        }
    }
}
