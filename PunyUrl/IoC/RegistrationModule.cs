using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.WindowsAzure.Storage;
using Module = Autofac.Module;

namespace PunyUrl.IoC
{
    public class RegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();

            builder.Register(ctx =>
            {
                var storageAccount = ConfigurationManager.AppSettings["StorageConnectionString"];

                var cloudStorageAccount = CloudStorageAccount.Parse(storageAccount);

                return cloudStorageAccount.CreateCloudTableClient();
            }).SingleInstance();

            base.Load(builder);
        }
    }
}