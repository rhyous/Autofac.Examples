using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfAndAutofac
{
    public partial class RestService1 : IRestService1
    {
        private readonly IHeaders _Headers;
        private readonly IUrlParameters _UrlParameters;
        private readonly IRequestUri _RequestUri;
        private readonly OperationContext _OperationContext;
        private readonly WebOperationContext _WebOperationContext;

        public RestService1(IHeaders headers,
                            IUrlParameters urlParameters,
                            IRequestUri requestUri,
                            OperationContext operationContext,
                            WebOperationContext webOperationContext)
        {
            _Headers = headers;
            _UrlParameters = urlParameters;
            _RequestUri = requestUri;
            _OperationContext = operationContext;
            _WebOperationContext = webOperationContext;
        }

        public User GetJsonData(string id, string username)
        {
            // Add a breakpoint here toinspect your Headers, UrlParameters, RequestUri,
            // OperationContext, etc.
            return new User { Id = Convert.ToInt32(id), Username = username };
        }
    }
}
