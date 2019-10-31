using System.Collections.Specialized;

namespace WcfAndAutofac
{
    public class UrlParameters : IUrlParameters
    {
        public NameValueCollection Collection { get; set; }
    }
}
