namespace Workshop
{
    public struct TestEvent : IEvent
    {
    }

    public struct TestEventWithArgs : IEvent
    {
        public string Pito;
    }

    public struct PlayerEvent : IEvent
    {
        public int Health;
    }
}