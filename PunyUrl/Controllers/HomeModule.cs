using Nancy;

namespace PunyUrl.Controllers
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["index.html"];
        }
    }
}