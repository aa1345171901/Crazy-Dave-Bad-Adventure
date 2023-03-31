using System;
using System.Collections.Generic;

namespace TopDownPlate
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
    }

    public class CustomState : IState
    {
        private Action onEnter;
        private Action onExit;
        private Action onUpdate;
        private Action onFixedUpdate;

        public CustomState OnEnter(Action action)
        {
            onEnter = action;
            return this;
        }

        public CustomState OnExit(Action action)
        {
            onExit = action;
            return this;
        }

        public CustomState OnUpdate(Action action)
        {
            onUpdate = action;
            return this;
        }

        public CustomState OnFixedUpdate(Action action)
        {
            onFixedUpdate = action;
            return this;
        }

        public void Enter()
        {
            onEnter?.Invoke();
        }

        public void Exit()
        {
            onExit?.Invoke();
        }

        public void Update()
        {
            onUpdate?.Invoke();
        }

        public void FixedUpdate()
        {
            onFixedUpdate?.Invoke();
        }
    }

    public class FSM<T>
    {
        private Dictionary<T, IState> stateDicts = new Dictionary<T, IState>();
        private IState currentState;
        private T currentStateId;

        public IState CurrentState => currentState;
        public T CurrentStateID => currentStateId;


        public CustomState AddState(T state)
        {
            if (stateDicts.ContainsKey(state))
            {
                return stateDicts[state] as CustomState;
            }
            var customState = new CustomState();
            stateDicts.Add(state, customState);
            return customState;
        }

        public void StartState(T state)
        {
            if (stateDicts.ContainsKey(state))
            {
                currentState = stateDicts[state];
                currentStateId = state;
                currentState.Enter();
            }
        }

        public void ChangeState(T state)
        {
            if (stateDicts.ContainsKey(state))
            {
                currentState?.Exit();
                currentState = stateDicts[state];
                currentStateId = state;
                currentState.Enter();
            }
        }

        public void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }

        public void Updaete()
        {
            currentState?.Update();
        }

        public void Clear()
        {
            currentState = null;
            currentStateId = default;
            stateDicts.Clear();
        }
    }
}