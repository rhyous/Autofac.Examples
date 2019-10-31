using Autofac.Core;
using Autofac.Integration.Wcf;
using System.Collections.Generic;

namespace WcfAndAutofac
{
    public class PerInstanceContextJitModuleContainer : IPerInstanceContextJitModuleContainer
    {
        public IEnumerable<IModule> Modules { get; } = new[] { new WcfPerIntanceContextModule() };
    }
}