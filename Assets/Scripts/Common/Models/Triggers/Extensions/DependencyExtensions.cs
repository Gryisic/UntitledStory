using Common.Models.Triggers.Dependencies;
using Common.Models.Triggers.Dependencies.Interfaces;
using Common.Units.Handlers;
using Core.Interfaces;

namespace Common.Models.Triggers.Extensions
{
    public static class DependencyExtensions
    {
        public static void SetUnitsHandler(this Dependency dependency, GeneralUnitsHandler handler)
        {
            if (dependency is IUnitBasedDependency handlerRequired)
                handlerRequired.SetHandler(handler);
        }

        public static void Respond(this Dependency dependency)
        {
            if (dependency is ITriggerResponder responder)
                responder.Respond();
        }
        
        public static void Activate(this Dependency dependency)
        {
            if (dependency is IActivatable activatable)
                activatable.Activate();
        }
        
        public static void Deactivate(this Dependency dependency)
        {
            if (dependency is IDeactivatable deactivatable)
                deactivatable.Deactivate();
        }
    }
}