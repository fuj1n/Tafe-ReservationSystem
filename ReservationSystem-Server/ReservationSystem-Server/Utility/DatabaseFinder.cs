using Microsoft.Data.SqlClient;

namespace ReservationSystem_Server.Utility;

public static class DatabaseFinder
{
    public static string? GetFirstAvailable(IEnumerable<string> connectionStrings)
    {
        foreach (string connectionString in connectionStrings)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
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