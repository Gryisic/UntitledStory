namespace Common.Units.Behaviour.Interfaces
{
    public interface IBehaviourChanger
    {
        void ChangeBehaviour<T>() where T: UnitBehaviour;
    }
}