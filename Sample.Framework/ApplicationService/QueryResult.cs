namespace Sample.Framework.ApplicationService
{
    public record QueryResult<T>(IEnumerable<T> Results, int Count, int TotalCount = default);
}
