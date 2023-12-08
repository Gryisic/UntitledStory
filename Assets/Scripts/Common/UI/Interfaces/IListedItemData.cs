using UnityEngine;

namespace Common.UI.Interfaces
{
    public interface IListedItemData
    {
        Sprite Icon { get; }
        string Name { get; }
        string Description { get; }
        int Cost { get; }
    }
}