using System.Collections.Generic;

namespace Infrastructure.Factories.Extensions
{
    public static class UnitFactoryExtensions
    {
        private static readonly Dictionary<int, string> _idNameMap = new Dictionary<int, string>()
        {
            {1, "Protagonist"},
        };

        public static string DefineUnit(this int id) => _idNameMap[id];
    }
}