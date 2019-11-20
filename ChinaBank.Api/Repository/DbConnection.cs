using Microsoft.Extensions.Configuration;
using System.IO;

public static class DbConnection
{
    private static string Db_Conn = "ConnectionStrings:ChinaDbContext";

    public static string getStringConnection(IConfiguration configuration)
    {
        return string.Format(
                               configuration.GetSection(DbConnection.Db_Conn).Value,
                               new DirectoryInfo(path: Directory.GetCurrentDirectory()).FullName
                            );        
    }
}
