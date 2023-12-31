﻿using System.Collections.Generic;

namespace Infrastructure.Factories.Extensions
{
    public static class UnitFactoryExtensions
    {
        private static readonly Dictionary<int, string> _idNameMap = new Dictionary<int, string>()
        {
            {1, "Protagonist_Explore"},
            {11, "Protagonist_Battle"}
        };

        public static string DefineUnit(this int id) => _idNameMap[id];
    }
}