using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacGenericExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(ServiceLoader<>)).As(typeof(IServiceLoader<>));
            builder.RegisterGeneric(typeof(ServiceProxy<,,>)).As(typeof(IService<,,>));
            var container = builder.Build();

            var proxyUserService = container.Resolve<IService<Organization, IOrganization, int>>();
            var user = proxyUserService.Get(27);
            var proxyOrganizationService = container.Resolve<IService<User, IUser, int>>();
            var org = proxyOrganizationService.Get(1011);
        }
    }

    public interface IBaseEntity<TId>
    {
        TId Id { get; set; }
    }

    public interface IUser : IBaseEntity<int>
    {
        string Username { get; set; }
    }

    public class User : IUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public interface IOrganization : IBaseEntity<int>
    {
        string Name { get; set; }
    }

    public class Organization : IOrganization
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface IService<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        TInterface Get(TId id);
    }

    public class Service<TEntity, TInterface, TId> : IService<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public virtual TInterface Get(TId id)
        {
            return new TEntity { Id = id };
        }
    }

    public class ServiceProxy<TEntity, TInterface, TId> : IService<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceLoader<IService<TEntity, TInterface, TId>> _ServiceLoader;

        public ServiceProxy(IServiceLoader<IService<TEntity, TInterface, TId>> serviceLoader)
        {
            _ServiceLoader = serviceLoader;
        }

        internal IService<TEntity, TInterface, TId> Service => _Service ?? (_Service = _ServiceLoader.Load());
        private IService<TEntity, TInterface, TId> _Service;

        public TInterface Get(TId id)
        {
            return Service.Get(id);
        }
    }

    public interface IServiceLoader<T>
    {
        T Load();
    }

    public class ServiceLoader<T> : IServiceLoader<T>
            where T : class
    {
        private readonly ILifetimeScope _Container;

        public ServiceLoader(ILifetimeScope container)
        {
            _Container = container;
        }
        public T Load()
        {
            using (var scope = _Container.BeginLifetimeScope(builder =>
            {
                var types = new[] { typeof(UserService), typeof(Service<Organization, IOrganization, int>) };
                foreach (var type in types)
                {

                    if (!type.IsGenericTypeDefinition)
                        builder.RegisterType(type).As(type.GetInterfaces().FirstOrDefault());
                    else
                        builder.RegisterGeneric(type.GetGenericTypeDefinition()).As(type.GetInterfaces().FirstOrDefault());

                }
            }))
            {
                if (typeof(T).IsGenericTypeDefinition)
                    return null;
                return scope.Resolve<T>(); ;
            }
        }
    }

    public class ServiceExtended<TEntity, TInterface, TId> : Service<TEntity, TInterface, TId>
    where TEntity : class, TInterface, new()
    where TInterface : IBaseEntity<TId>
    where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public virtual TInterface Set(TEntity entity)
        {
            return entity;
        }
    }

    public class UserService : Service<User, IUser, int>
    {
        public override IUser Get(int id)
        {
            return new User();
        }
    }
}
