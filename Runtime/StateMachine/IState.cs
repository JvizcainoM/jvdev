namespace JvDev.StateMachine
{
    public interface IState
    {
        void Enter(IState lastState);
        void Exit(IState nextState);
        void Update();
        void FixedUpdate();
    }
}