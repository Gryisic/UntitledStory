namespace Core.Interfaces
{
    public interface IStateChanger<in T> where T: class, IChangeableState
    {
        void ChangeState<TState>() where TState: T;
    }
}