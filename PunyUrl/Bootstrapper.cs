using Nancy;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace PunyUrl
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            this.Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("Views/", viewName));
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.AddDirectory("Scripts", "Scripts", "js", "js.map");
            conventions.StaticContentsConventions.AddDirectory("Content", "Content", "css", "css.map");
        }
    }
}