using Autofac;
using System;

namespace AutofacScopeExample
{
    public abstract class BaseService
    {
        private readonly int _instanceNumber;

        public BaseService(int instance)
        {
            _instanceNumber = instance;
            Console.WriteLine($"Just created {this.GetType().Name} #{_instanceNumber}");
        }

        public void Dispose() => Console.WriteLine($"Disposing {this.GetType().Name} #{_instanceNumber}");
    }

    public class TransientService : BaseService 
    {
        public static int _instanceCount;
        public TransientService() : base(++_instanceCount) { }
    }

    public class LifetimeScopeService : BaseService
    {
        public static int _instanceCount;
        public LifetimeScopeService() : base(++_instanceCount) { }
    }

    public class SingletonService : BaseService
    {
        public static int _instanceCount;
        public SingletonService() : base(++_instanceCount) { }
    }

    public class OuterScopedSingletonService : BaseService
    {
        public static int _instanceCount;
        public OuterScopedSingletonService() : base(++_instanceCount) { }
    }

    public class InnerScopedSingletonService : BaseService
    {
        public static int _instanceCount;
        public InnerScopedSingletonService() : base(++_instanceCount) { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<TransientService>()
                .AsSelf()
                .InstancePerDependency();

            builder
                .RegisterType<LifetimeScopeService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<SingletonService>()
                .AsSelf()
                .SingleInstance();

            using (var container = builder.Build())
            {
                Console.WriteLine("Created the root scope");

                var rootTransientService = container.Resolve<TransientService>();
                var rootLifetimeScopeService = container.Resolve<LifetimeScopeService>();
                var rootSingletonService = container.Resolve<SingletonService>();

                var rootTransientServiceTwo = container.Resolve<TransientService>();
                var rootLifetimeScopeServiceTwo = container.Resolve<LifetimeScopeService>();
                var rootSingletonServiceTwo = container.Resolve<SingletonService>();

                using (var outerLifetimeScope = container.BeginLifetimeScope(outerBuilder =>
                {
                    outerBuilder.RegisterType<OuterScopedSingletonService>().AsSelf().SingleInstance();
                }))
                {
                    Console.WriteLine("Created the outer lifetime scope");

                    var outerTransientService = outerLifetimeScope.Resolve<TransientService>();
                    var outerLifetimeScopeService = outerLifetimeScope.Resolve<LifetimeScopeService>();
                    var outerSingletonService = outerLifetimeScope.Resolve<SingletonService>();
                    var outerOuterScopedSingletonService = outerLifetimeScope.Resolve<OuterScopedSingletonService>();

                    var outerTransientServiceTwo = outerLifetimeScope.Resolve<TransientService>();
                    var outerLifetimeScopeServiceTwo = outerLifetimeScope.Resolve<LifetimeScopeService>();
                    var outerSingletonServiceTwo = outerLifetimeScope.Resolve<SingletonService>();
                    var outerOuterScopedSingletonServiceTwo = outerLifetimeScope.Resolve<OuterScopedSingletonService>();

                    for (int i = 1; i <= 3; i++)
                    {
                        using (var innerLifetimeScope = outerLifetimeScope.BeginLifetimeScope(innerBuilder =>
                        {
                            innerBuilder.RegisterType<InnerScopedSingletonService>().AsSelf().SingleInstance();
                        }))
                        {
                            Console.WriteLine($"Created the inner lifetime scope {i}");

                            var innerTransientService = innerLifetimeScope.Resolve<TransientService>();
                            var innerLifetimeScopeService = innerLifetimeScope.Resolve<LifetimeScopeService>();
                            var innerSingletonService = innerLifetimeScope.Resolve<SingletonService>();
                            var innerOuterScopedSingletonService = innerLifetimeScope.Resolve<OuterScopedSingletonService>();
                            var innerInnerScopedSingletonService = innerLifetimeScope.Resolve<InnerScopedSingletonService>();

                            var innerTransientServiceTwo = innerLifetimeScope.Resolve<TransientService>();
                            var innerLifetimeScopeServiceTwo = innerLifetimeScope.Resolve<LifetimeScopeService>();
                            var innerSingletonServiceTwo = innerLifetimeScope.Resolve<SingletonService>();
                            var innerOuterScopedSingletonServiceTwo = innerLifetimeScope.Resolve<OuterScopedSingletonService>();
                            var innerInnerScopedSingletonServiceTwo = innerLifetimeScope.Resolve<InnerScopedSingletonService>();
                        }
                        Console.WriteLine("Disposed the inner lifetime scope");
                    }

                }

                Console.WriteLine("Disposed the outer lifetime scope");
            }

            Console.WriteLine("Disposed the root scope");

            Console.ReadLine();
        }
    }
}
