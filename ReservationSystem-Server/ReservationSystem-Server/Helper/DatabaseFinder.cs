using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace ReservationSystem_Server.Helper;

public static class DatabaseFinder
{
    private static readonly Regex StripDb = new("(Database|InitialCatalog)=.+?;", RegexOptions.Compiled); 
    
    public static string? GetFirstAvailable(IEnumerable<(string, string)> connectionStrings)
    {
        foreach ((string name, string connectionString) in connectionStrings)
        {
            try
            {
                Console.WriteLine("Trying connection string " + name);
                using var connection = new SqlConnection(StripDb.Replace(connectionString, ""));
                connection.Open();
                return connectionString;
            }
            catch
            {
                // ignored
            }
        }

        return null;
    }
}