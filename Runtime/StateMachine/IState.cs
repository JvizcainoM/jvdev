namespace JV.StateMachine
{
    public interface IState
    {
        byte Id { get; }
        void Enter(IState lastState = null);
        void Exit(IState nextState = null);
        void Update();
        void FixedUpdate();
    }
}