using System.Collections.Generic;

namespace Infrastructure.Factories.Extensions
{
    public static class UnitFactoryExtensions
    {
        private static readonly Dictionary<int, string> _idNameMap = new()
        {
            {1, "Protagonist"},
            {-999, "Dummy"}
        };

        public static string DefineUnit(this int id) => _idNameMap[id];
    }
}