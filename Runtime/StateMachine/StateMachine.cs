using System;
using System.Collections.Generic;
using System.Linq;

namespace JvDev.StateMachine
{
    public class StateMachine
    {
        private StateNode _currentNode;
        private readonly Dictionary<Type, StateNode> _nodes = new();
        private readonly HashSet<ITransition> _anyTransitions = new();
        public IState CurrentState => _currentNode.State;

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null) ChangeState(transition.To);
            _currentNode.State.Update();
        }

        public void FixedUpdate()
        {
            _currentNode.State.FixedUpdate();
        }

        public void SetState(IState state)
        {
            _currentNode = _nodes[state.GetType()];
            _currentNode.State.Enter(null);
        }

        public void ChangeState(Type newStateType)
        {
            if (newStateType == _currentNode.State.GetType())
                return;

            var lastState = _currentNode.State;
            _currentNode = _nodes[newStateType];
        
            lastState.Exit(_currentNode.State);
            _currentNode.State.Enter(lastState);
        }

        public void ChangeState(IState newState)
        {
            if (newState == _currentNode.State)
                return;

            var lastState = _currentNode.State;
        
            _currentNode.State.Exit(newState);
            _currentNode = _nodes[newState.GetType()];
            _currentNode.State.Enter(lastState);
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.IsSatisfied())
                    return transition;
            }

            return _currentNode.Transitions.FirstOrDefault(transition => transition.Condition.IsSatisfied());
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetNode(from).AddTransition(GetNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetNode(to).State, condition));
        }

        private StateNode GetNode(IState state)
        {
            if (_nodes.TryGetValue(state.GetType(), out var node))
                return node;

            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
            return node;
        }
    }

    internal class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }

        public StateNode(IState state)
        {
            State = state;
            Transitions = new HashSet<ITransition>();
        }

        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
}