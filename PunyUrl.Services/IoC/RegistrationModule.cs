using Autofac;

namespace PunyUrl.Services.IoC
{
    public class RegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Base62UrlShortener>().As<IUrlShortener>().SingleInstance();
            builder.RegisterType<AzureTableUrlStorage>().As<IUrlStorage<Domain.Entities.PunyUrl>>().SingleInstance();
            builder.RegisterType<AzureTableUrlMetadataProvider>().As<IUrlMetadataProvider>().SingleInstance();

            base.Load(builder);
        }
    }
}