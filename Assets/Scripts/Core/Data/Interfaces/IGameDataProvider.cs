﻿namespace Core.Data.Interfaces
{
    public interface IGameDataProvider
    {
        public T GetData<T>() where T: IGameData;
    }
}