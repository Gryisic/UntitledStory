using UnityEngine;

namespace Infrastructure.Utils.Tools
{
    public class SubclassesPicker : PropertyAttribute
    {
        public bool DrawLabel { get; }
        public bool DrawChilds { get; }
        
        /// <summary>
        /// Used for picking classes derived from current type. Requires "SerializedReference" attribute.
        /// </summary>
        /// <param name="drawChilds">Draw or hide childs?</param>
        /// <param name="drawLabel">Draw or hide label?</param>
        public SubclassesPicker(bool drawChilds = true, bool drawLabel = false)
        {
            DrawChilds = drawChilds;
            DrawLabel = drawLabel;
        }
    }
}