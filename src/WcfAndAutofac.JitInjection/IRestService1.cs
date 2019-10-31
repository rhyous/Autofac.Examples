using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfAndAutofac
{
    [ServiceContract]
    public interface IRestService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetJsonData/{id}/{username}", ResponseFormat = WebMessageFormat.Json)]
        User GetJsonData(string id, string username); // i.e RESTful service
    }
}