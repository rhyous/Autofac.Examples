using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfAndAutofac
{
    public class Settings : ISettings
    {
        public string Token { get; set; } = "Example_TOKEN_01";
    }
}