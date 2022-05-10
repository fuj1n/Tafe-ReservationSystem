namespace ReservationSystem_Server.Helper;

public static class QueryHelper
{
    /// <summary>
    /// Applies a given filter to a query.
    /// </summary>
    /// <param name="query">The query to apply the filter to</param>
    /// <param name="filter">The filter to apply</param>
    /// <typeparam name="T">The type of the query</typeparam>
    /// <returns>The filtered query (or original query if filter is null)</returns>
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query,
        Func<IQueryable<T>, IQueryable<T>>? filter)
    {
        if (filter != null)
        {
            query = filter(query);
        }

        return query;
    }
}