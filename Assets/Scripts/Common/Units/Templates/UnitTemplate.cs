using UnityEngine;

namespace Common.Units.Templates
{
    public abstract class UnitTemplate : ScriptableObject
    {
        [SerializeField] private int _id;

        public int ID => _id;
    }
}