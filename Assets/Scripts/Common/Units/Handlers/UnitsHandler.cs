using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models.Scene;
using Common.Units.Extensions;
using Common.Units.Interfaces;

namespace Common.Units.Handlers
{
    public abstract class UnitsHandler<T> : IDisposable where T: class, IUnit
    {
        protected readonly UnitsPool unitsPool;
        protected readonly List<IUnit> units;
        
        protected bool isDirty;

        protected UnitsHandler(UnitsPool unitsPool)
        {
            this.unitsPool = unitsPool;
            
            units = new List<IUnit>();
            
            isDirty = true;
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
            {
                units.Add(unit);

                isDirty = true;
            }
            
            return unit;
        }

        public virtual bool TryRemove(IUnit unit)
        {
            if (unit == null)
                throw new NullReferenceException("Trying to remove unit that is null");

            if (units.Contains(unit))
            {
                unitsPool.Return(unit);
                units.Remove(unit);

                isDirty = true;

                return true;
            }

            return false;
        }

        public virtual void Clear()
        {
            units.ForEach(u => unitsPool.Return(u));
            
            units.Clear();
            
            isDirty = true;
        }

        public void ActivateAll() => units.ForEach(u => u.ActivateAndShow());
        
        public void DeactivateAll() => units.ForEach(u => u.DeactivateAndHide());
    }
}