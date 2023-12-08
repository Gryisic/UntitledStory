using System;
using System.Collections.Generic;
using Common.Units.Extensions;

namespace Common.Units.Handlers
{
    public abstract class UnitsHandler<T> : IDisposable where T: Unit
    {
        protected readonly List<Unit> units;

        protected UnitsHandler()
        {
            units = new List<Unit>();
        }
        
        public void Dispose()
        {
            foreach (var unit in units)
            {
                if (unit is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public virtual void Add(Unit unit)
        {
            if (unit == null || unit is T == false)
                throw new NullReferenceException($"Trying to add unit that is null or unit is not an '{typeof(T)}'");
            
            units.Add(unit);
        }

        public virtual bool TryRemove(Unit unit)
        {
            if (unit == null)
                throw new NullReferenceException("Trying to remove unit that is null");

            if (units.Contains(unit))
            {
                units.Remove(unit);

                return true;
            }

            return false;
        }

        public virtual void Clear() => units.Clear();

        public void ActivateAll() => units.ForEach(u => u.ActivateAndShow());
        
        public void DeactivateAll() => units.ForEach(u => u.DeactivateAndHide());
    }
}