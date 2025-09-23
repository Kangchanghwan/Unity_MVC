using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    public interface IState<T>
    {
        UniTask<T> Enter(T previousState);
        UniTask Exit(T nextState);
    }

    public class StateMachine
    {
        private readonly bool _isLogging = false;
        public StateMachine(bool isLogging)
        {
            _isLogging = isLogging;
        }

        public async UniTask Execute<T>(T state) where T : class, IState<T>
        {
            T previousState = null;
            T currentState = state;
            while (currentState != null)
            {
                if (previousState != null)
                {
                    await previousState.Exit(currentState);
                }
                if(_isLogging)
                {
                    Debug.Log($"새로운 상태 진입 : {currentState.GetType().Name}");
                }
                var nextState = await currentState.Enter(previousState);
                previousState = currentState;
                currentState = nextState;
            }
        }
    }
}