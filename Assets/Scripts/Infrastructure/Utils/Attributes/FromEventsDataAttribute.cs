using System;
using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FromEventsDataAttribute : PropertyAttribute
    {
        /// <summary>
        /// FOR EVENTS ONLY!!! Fills field with corresponding data from events database.
        /// </summary>
        public FromEventsDataAttribute() { }
    }
}