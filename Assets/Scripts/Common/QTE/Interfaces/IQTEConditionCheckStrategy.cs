using System;
using Infrastructure.Utils;

namespace Common.QTE.Interfaces
{
    public interface IQTEConditionCheckStrategy
    {
        event Action Succeeded;
        event Action Failed;

        void Start();
        void Input(Enums.QTEState state, Enums.QTEInput input);
        void CancelInput(Enums.QTEState state);
    }
}