using System;
using System.Collections.Generic;

namespace Common.Units
{
    public class UnitsHandler : IDisposable
    {
        private List<Unit> _units;

        public UnitsHandler()
        {
            _units = new List<Unit>();
        }
        
        public void Dispose()
        {
            foreach (var unit in _units)
            {
                if (unit is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        public void Add(Unit unit)
        {
            if (unit == null)
                throw new NullReferenceException("Trying to add unit that is null");
            
            _units.Add(unit);
        }

        public bool TryRemove(Unit unit)
        {
            if (unit == null)
                throw new NullReferenceException("Trying to remove unit that is null");

            if (_units.Contains(unit))
            {
                _units.Remove(unit);

                return true;
            }

            return false;
        }
    }
}