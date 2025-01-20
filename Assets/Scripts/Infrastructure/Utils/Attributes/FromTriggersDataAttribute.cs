using UnityEngine;

namespace Infrastructure.Utils.Attributes
{
    public class FromTriggersDataAttribute : PropertyAttribute
    {
        public bool AsDropdown { get; }
        public bool SuppressGUI { get; }

        /// <summary>
        /// FOR TRIGGERS ONLY!!! Fills field with corresponding data from triggers database.
        /// </summary>
        /// <param name="asDropdown">ID ONLY!!! Generate dropdown menu? Put true if needed to fill id.</param>
        /// <param name="suppressGUI">Ignore if used with id.</param>
        public FromTriggersDataAttribute(bool asDropdown, bool suppressGUI = false)
        {
            AsDropdown = asDropdown;
            SuppressGUI = suppressGUI;
        }

        /// <summary>
        /// FOR TRIGGERS ONLY!!! Fills field with corresponding data from triggers database.
        /// </summary>
        /// <param name="suppressGUI">Suppress GUI?.</param>
        public FromTriggersDataAttribute(bool suppressGUI = true)
        {
            SuppressGUI = suppressGUI;
        }
    }
}