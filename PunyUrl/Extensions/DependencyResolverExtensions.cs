namespace PunyUrl.Extensions
{
    public static class DependencyResolverExtensions
    {
        public static TService GetService<TService>(this System.Web.Http.Dependencies.IDependencyResolver dependencyResolver)
        {
            return (TService)dependencyResolver.GetService(typeof(TService));
        }
    }
}