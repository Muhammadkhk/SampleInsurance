using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CoreInspect.Core.API.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public SqlHealthCheck(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("Database");
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
