public class Connection
{
    private static string Server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
    private static string Database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "dbClassSchedule";
    private static string Uid = Environment.GetEnvironmentVariable("DB_UID") ?? "user";
    private static string Pwd = Environment.GetEnvironmentVariable("DB_PWD") ?? "secret";
    private static string Port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";

    public static string CONNECTION_STRING => $"Server={Server};Database={Database};Uid={Uid};Pwd={Pwd};port={Port};SslMode=Required;";
}
