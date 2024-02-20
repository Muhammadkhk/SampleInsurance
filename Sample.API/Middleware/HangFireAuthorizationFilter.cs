using Hangfire.Dashboard;

namespace Sample.API.Middleware
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //var httpContext = context.GetHttpContext();

            //// Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity?.IsAuthenticated ?? false;
            return true;
        }
    }
}
