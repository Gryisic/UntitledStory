using System;
using Infrastructure.Utils;

namespace Common.QTE.EndBehaviour
{
    public class QTESuppressArgs : EventArgs
    {
        public Enums.QTESuppress Suppress { get; }
        public Enums.QTEDataUpdate DataUpdate { get; }
        
        public QTESuppressArgs(Enums.QTESuppress suppress, Enums.QTEDataUpdate dataUpdate)
        {
            Suppress = suppress;
            DataUpdate = dataUpdate;
        }
    }
}