namespace BL
{
    public class ConnectionString
    {
        private static string Connection { get; set; }
        public ConnectionString(string connectionString)
        {
            Connection = connectionString;
        }

        public static string GetConnectionString()
        {
            return Connection;
        }
    }
}
