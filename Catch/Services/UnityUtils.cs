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

            foreach (var implTypeInfo in implTypeInfos)
            {
                var implType = implTypeInfo.AsType();

                // Unity does not share instances for multiple registrations when using 
                // named registrations, so we need to instantiate it now and register the 
                // instances instead.
                var inst = container.Resolve(implType);

                // register as the concrete type
                container.RegisterInstance(implType, inst);

                // register as the interface type (for resolve all)
                container.RegisterInstance(ofType, implTypeInfo.Name, inst);
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
