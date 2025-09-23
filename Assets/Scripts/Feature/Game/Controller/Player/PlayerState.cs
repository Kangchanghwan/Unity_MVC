using System;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Feature.Game.Controller.Player
{
    public abstract class PlayerStateBase : IState<PlayerStateBase>
    {
        public abstract UniTask<PlayerStateBase> Enter(PlayerStateBase previousState);

        public virtual UniTask Exit(PlayerStateBase nextState)
        {
            return UniTask.CompletedTask;
        }
    }

    public class IdleState : PlayerStateBase
    {
        public override UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
        {
            Debug.Log("Idle 상태 진입");
            return UniTask.FromResult<PlayerStateBase>(null);
        }
    }

    public class JumpState : PlayerStateBase
    {
        private readonly PlayerController _controller;

        public JumpState(PlayerController controller)
        {
            this._controller = controller;
        }

        public override async UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
        {
            Debug.Log("Jump 상태 진입");
            
            // 점프 실행
            _controller.ExecuteJump();
            
            // 착지할 때까지 대기
            await UniTask.WaitUntil(() => _controller.IsGrounded());
            
            Debug.Log("착지 완료, Idle로 전환");
            return new IdleState();
        }
    }

    public class DiedState : PlayerStateBase
    {
        private readonly PlayerController _controller;

        public DiedState(PlayerController controller)
        {
            _controller = controller;
        }

        public override async UniTask<PlayerStateBase> Enter(PlayerStateBase previousState)
        {
            Debug.Log("Die 상태 진입 - 게임 오버");
            
            // 죽음 애니메이션 길이만큼 대기 (실제 애니메이션 길이에 맞춤)
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            
            UnityEngine.Object.Destroy(_controller.gameObject);
            Debug.Log("Die 상태 끝 - 개체 삭제됨");
            return null;
        }

    }
}