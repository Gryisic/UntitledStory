﻿using Common.Models.GameEvents.Dependencies.Interfaces;
using Common.Units.Handlers;
using Core.Interfaces;

namespace Common.Models.GameEvents.Dependencies.Extensions
{
    public static class DependencyExtensions
    {
        public static void SetUnitsHandler(this Dependency dependency, GeneralUnitsHandler handler)
        {
            if (dependency is IUnitBasedDependency handlerRequired)
                handlerRequired.SetHandler(handler);
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