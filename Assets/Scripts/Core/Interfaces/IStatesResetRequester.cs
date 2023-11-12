using System;

namespace Core.Interfaces
{
    public interface IStatesResetRequester
    {
        event Action ResetRequested;
    }
}