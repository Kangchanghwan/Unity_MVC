using Common;
using Cysharp.Threading.Tasks;
using Feature.Common;
using Feature.Game.Controller.Scenes.Feature.Game.Events;
using Feature.Game.Model;
using Feature.Game.View;
using Scenes.Feature.Game.Controller;
using UnityEngine;

namespace Feature.Game.Controller.Player
{
  /// <summary>
    /// 플레이어의 Model과 View를 중재하는 Controller
    /// </summary>
    public class PlayerController : EventListenerMono, IMonoEventDispatcher
    {
        [Header("Player Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private int maxHealth = 3;

        [Header("Components")]
        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerInputHandler playerInputHandler;

        private PlayerModel _model;
        private StateMachine _stateMachine;

        private void Awake()
        {
            _model = new PlayerModel(maxHealth);
            _stateMachine = new StateMachine(isLogging: true);
            
            if (playerView == null) playerView = GetComponent<PlayerView>();
            if (playerInputHandler == null) playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            // Model의 C# 이벤트 구독
            _model.OnHealthChanged += OnHealthChanged;
            _model.OnPlayerDied += OnPlayerDied;
            
            // 글로벌 이벤트 구독
            EventManager.Subscribe<ObjectDamagedEvent>(this);
            
            // 초기 상태 설정
            _stateMachine.Execute<PlayerStateBase>(new IdleState()).Forget();
        }

        // Model에서 오는 체력 변경 이벤트
        private void OnHealthChanged(int current, int max)
        {
            playerView.UpdateHealthUI(current, max);
            
            // 글로벌 이벤트 발행
            this.EmitGlobal(new PlayerHealthChangedEvent(current, max));
            
            if (current < max)
            {
                playerView.PlayDamageEffect();
            }
        }

        // Model에서 오는 죽음 이벤트
        private void OnPlayerDied()
        {
            playerView.PlayDeathAnimation();
            
            // 글로벌 이벤트 발행
            this.EmitGlobal(new PlayerDiedEvent("Player",transform.position));
            
            // 죽음 상태로 전환
            _stateMachine.Execute<PlayerStateBase>(new DiedState(this)).Forget();
        }

        // 글로벌 이벤트 핸들러
        public override EventChain OnEventHandle(IEvent @event)
        {
            switch (@event)
            {
                case ObjectDamagedEvent damageEvent:
                    // 자신에게 온 데미지인지 확인
                    if (string.IsNullOrEmpty(damageEvent.TagName) || damageEvent.TagName == "Player")
                    {
                        Debug.Log($"플레이어 Player 이 {damageEvent.Damage} 데미지를 받음");
                        _model.ApplyDamage(damageEvent.Damage);
                        return EventChain.Break;
                    }
                    break;

                case PlayerJumpEvent:
                    if (!_model.IsDead && playerView.IsGrounded())
                    {
                        Debug.Log("점프 실행");
                        _stateMachine.Execute<PlayerStateBase>(new JumpState(this)).Forget();
                        return EventChain.Break;
                    }
                    break;
            }
            return EventChain.Continue;
        }

        // State에서 호출하는 메서드들
        public void ExecuteJump()
        {
            playerView.ApplyJumpForce(jumpForce);
            playerView.PlayJumpAnimation();
        }
    
        public bool IsGrounded() => playerView.IsGrounded();
        public bool IsDead() => _model.IsDead;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_model == null) return;
            _model.OnHealthChanged -= OnHealthChanged;
            _model.OnPlayerDied -= OnPlayerDied;
        }
    }
}