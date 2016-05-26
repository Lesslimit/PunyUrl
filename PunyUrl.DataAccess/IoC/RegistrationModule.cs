using Autofac;
using PunyUrl.DataAccess.Azure;

namespace PunyUrl.DataAccess.IoC
{
    public class RegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TableStorage>().As<ITableStorage>().SingleInstance();
        }
    }
}