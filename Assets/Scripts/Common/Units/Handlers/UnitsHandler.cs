using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Scene;
using Common.Units.Extensions;

namespace Common.Units.Handlers
{
    public abstract class UnitsHandler<T> : IDisposable where T: Unit
    {
        protected readonly UnitsPool unitsPool;
        protected readonly List<Unit> units;

        protected UnitsHandler(UnitsPool unitsPool)
        {
            this.unitsPool = unitsPool;
            
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

        public T GetUnitWithID(int id)
        {
            T unit = unitsPool.GetUnitOfTypeWithID<T>(id);
            
            if (units.Contains(unit) == false)
                units.Add(unit);
            
            return unit;
        }

        public virtual bool TryRemove(Unit unit)
        {
            if (unit == null)
                throw new NullReferenceException("Trying to remove unit that is null");

            if (units.Contains(unit))
            {
                unitsPool.Return(unit);
                units.Remove(unit);

                return true;
            }

            return false;
        }

        public virtual void Clear()
        {
            units.ForEach(u => unitsPool.Return(u));
            
            units.Clear();
        }

        public void ActivateAll() => units.ForEach(u => u.ActivateAndShow());
        
        public void DeactivateAll() => units.ForEach(u => u.DeactivateAndHide());
    }
}