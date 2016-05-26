using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using PunyUrl;
using PunyUrl.Extensions;
using PunyUrl.IoC;
using PunyUrl.Middleware;
using PunyUrl.Services;

[assembly: OwinStartup(typeof(Startup))]
namespace PunyUrl
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = GlobalConfiguration.Configuration;

            httpConfig.MapHttpAttributeRoutes();
            httpConfig.EnsureInitialized();

            var builder = new ContainerBuilder();

            builder.RegisterModule<RegistrationModule>();
            builder.RegisterModule<DataAccess.IoC.RegistrationModule>();
            builder.RegisterModule<Services.IoC.RegistrationModule>();

            httpConfig.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());

            app.UseAutofacWebApi(httpConfig)
               .UseWebApi(httpConfig)
               .Use<RedirectMiddleware>(httpConfig.DependencyResolver.GetService<IUrlStorage<Domain.Entities.PunyUrl>>())
               .UseNancy(no => no.Bootstrapper = new Bootstrapper());

            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}