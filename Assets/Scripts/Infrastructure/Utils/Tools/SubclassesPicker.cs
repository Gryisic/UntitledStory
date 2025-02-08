using UnityEngine;

namespace Infrastructure.Utils.Tools
{
    public class SubclassesPicker : PropertyAttribute
    {
        public bool DrawLabel { get; }
        public bool DrawChilds { get; }
        public bool DisableManualChange { get; }

        /// <summary>
        /// Used for picking classes derived from current type. Requires "SerializedReference" attribute.
        /// </summary>
        /// <param name="drawChilds">Draw or hide childs?</param>
        /// <param name="drawLabel">Draw or hide label?</param>
        /// <param name="disableManualChange">Enable or disable manual field change?</param>
        public SubclassesPicker(bool drawChilds = true, bool drawLabel = false, bool disableManualChange = false)
        {
            DisableManualChange = disableManualChange;
            DrawChilds = drawChilds;
            DrawLabel = drawLabel;
        }
    }
}