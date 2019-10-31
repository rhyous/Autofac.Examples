using Autofac;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfAndAutofac
{
    public class WcfPerIntanceContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(OperationContext.Current).SingleInstance();

            builder.RegisterInstance(WebOperationContext.Current).SingleInstance();

            var headers = new Headers { Collection = WebOperationContext.Current?.IncomingRequest?.Headers };
            builder.RegisterInstance(headers).As<IHeaders>().SingleInstance();

            var urlParameters = new UrlParameters { Collection = WebOperationContext.Current?.IncomingRequest?.UriTemplateMatch?.QueryParameters };
            builder.RegisterInstance(urlParameters).As<IUrlParameters>().SingleInstance();

            var requestedUri = new RequestUri { Uri = OperationContext.Current?.RequestContext?.RequestMessage?.Headers.To };
            builder.RegisterInstance(requestedUri).As<IRequestUri>().SingleInstance();
        }
    }
}