public class Connection
{
    private static string Server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost" ?? "capstoneteam6db.mysql.database.azure.com";
    private static string Database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "dbClassSchedule";
    private static string Uid = Environment.GetEnvironmentVariable("DB_UID") ?? "user" ?? "DylanMac";
    private static string Pwd = Environment.GetEnvironmentVariable("DB_PWD") ?? "secret" ?? "Wer45Tgbvf";
    private static string Port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";

    private static string sslPath = Environment.GetEnvironmentVariable("SSL_PATH") ?? "ssl/DigiCertGlobalRootCA.crt.pem";

    public static string CONNECTION_STRING => $"Server={Server};Database={Database};Uid={Uid};Pwd={Pwd};port={Port};SslMode=Required;SslCa={sslPath};";
}
