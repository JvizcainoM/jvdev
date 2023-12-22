namespace Workshop
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
    }
}