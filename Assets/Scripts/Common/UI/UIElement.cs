using UnityEngine;

namespace Common.UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public RectTransform Transform => transform as RectTransform;
        
        public virtual void Activate() => gameObject.SetActive(true);
        public virtual void Deactivate() => gameObject.SetActive(false);
    }
}