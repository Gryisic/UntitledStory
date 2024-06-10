using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.QTE.EndBehaviour
{
    public class SuppressSequenceBehaviour : QTEEndBehaviour
    {
        [SerializeField] private SuppressData _onSuccess;
        [SerializeField] private SuppressData _onFailure;

        public event Action<QTESuppressArgs> SequenceSuppressed; 

        public override void Update(Enums.QTEState qteState)
        {
            switch (qteState)
            {
                case Enums.QTEState.Succeeded:
                    SequenceSuppressed?.Invoke(new QTESuppressArgs(_onSuccess.Suppress, _onSuccess.DataUpdate));
                    break;
                
                case Enums.QTEState.Failed:
                    SequenceSuppressed?.Invoke(new QTESuppressArgs(_onFailure.Suppress, _onFailure.DataUpdate));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(qteState), qteState, null);
            }
        }

        public override void Clear() => SequenceSuppressed = null;
        
        [Serializable]
        private struct SuppressData
        {
            [SerializeField] private Enums.QTESuppress _suppress;
            [SerializeField] private Enums.QTEDataUpdate _dataUpdate;

            public Enums.QTESuppress Suppress => _suppress;
            public Enums.QTEDataUpdate DataUpdate => _dataUpdate;
        }
    }
}