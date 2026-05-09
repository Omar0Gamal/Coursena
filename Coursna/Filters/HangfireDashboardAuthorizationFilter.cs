using Hangfire.Dashboard;

namespace Coursna.Filters
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();


            if (!httpContext.User.Identity.IsAuthenticated)
                return false;


            return httpContext.User.IsInRole("Admin") ;
        }
    }
}
