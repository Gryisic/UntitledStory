using System;
using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AsFileNameAttribute : PropertyAttribute
    {
        /// <summary>
        /// Set string field value same as asset name. USE ONLY FOR 'STRING' FIELDS OF ASSETS!!! 
        /// </summary>
        public AsFileNameAttribute() { }
    }
}