using System;

namespace JV.StateMachine
{
    public class FuncPredicate : IPredicate
    {
        private readonly Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }

        public bool IsSatisfied() => _func.Invoke();
    }
}