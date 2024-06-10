using System;
using Infrastructure.Utils;

namespace Common.QTE.EndBehaviour
{
    [Serializable]
    public abstract class QTEEndBehaviour
    {
        public abstract void Update(Enums.QTEState qteState);

        public abstract void Clear();
    }
}