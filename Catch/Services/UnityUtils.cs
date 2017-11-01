using System;
using System.Linq;
using System.Reflection;
using Catch.Level;
using Unity;
using Unity.Lifetime;

namespace Catch.Services
{
    public static class UnityUtils
    {
        public static void RegisterAllAsSingletons(Type ofType, IUnityContainer container)
        {
            // Get the current assembly through the current class
            var currentAssembly = typeof(UnityUtils).GetTypeInfo().Assembly;

            // Filter the defined classes according to the interfaces they implement
            var implTypeInfos = currentAssembly
                .DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(inter => inter == ofType));

            // First register the concrete types
            foreach (var implTypeInfo in implTypeInfos)
            {
                var implType = implTypeInfo.AsType();

                // register as the concrete type
                container.RegisterType(implType, new ContainerControlledLifetimeManager());
            }

            // Then, instantiate and register by name so we can looked them all up in ResolveAll
            foreach (var implTypeInfo in implTypeInfos)
            {
                var implType = implTypeInfo.AsType();

                // register as the interface type (for resolve all)
                container.RegisterInstance(ofType, implTypeInfo.Name, container.Resolve(implType));
            }
        }

        public static void RegisterAllAsTransient(Type ofType, IUnityContainer container)
        {
            // Get the current assembly through the current class
            var currentAssembly = typeof(BuiltinAgentProvider).GetTypeInfo().Assembly;

            // Filter the defined classes according to the interfaces they implement
            var implTypeInfos = currentAssembly
                .DefinedTypes
                .Where(type => type.ImplementedInterfaces.Any(inter => inter == ofType));

            foreach (var implTypeInfo in implTypeInfos)
            {
                var implType = implTypeInfo.AsType();

                // register as the concrete type
                container.RegisterType(implType, new TransientLifetimeManager());

                // register as the interface type (for resolve by name)
                container.RegisterType(ofType, implType, implType.Name, new TransientLifetimeManager());
            }
        }
    }
}
