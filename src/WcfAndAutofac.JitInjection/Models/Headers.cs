using System.Collections.Specialized;

namespace WcfAndAutofac
{
    public class Headers : IHeaders
    {
        public NameValueCollection Collection { get; set; }
    }
}
