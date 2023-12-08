using UnityEngine;

namespace Common.Battle.Turns
{
    public class PartyMemberTurn : Turn
    {
        public override void Activate()
        {
            Debug.Log("PMT started");
        }

        public override void Deactivate()
        {
            Debug.Log("PMT ended");
        }
    }
}