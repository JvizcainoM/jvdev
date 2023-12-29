namespace JV.StateMachine
{
    public interface IState
    {
        byte Id { get; }
        void Enter(IState lastState);
        void Exit(IState nextState);
        void Update();
        void FixedUpdate();
    }
}