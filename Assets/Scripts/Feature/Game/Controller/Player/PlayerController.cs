using Cysharp.Threading.Tasks;
using Feature.Common;
using Scenes.Common;
using Scenes.Feature.Game.Model;
using Scenes.Feature.Game.View;
using UnityEngine;

namespace Scenes.Feature.Game.Controller
{
    public class PlayerController : MonoBehaviour, IMonoEventListener
    {
        [Header("Physics Settings")] public float jumpForce = 10f;

        [SerializeField] private PlayerView playerView;
        [SerializeField] private PlayerModel playerModel;

        [SerializeField] private PlayerInputHandler playerInputHandler;

        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private StateMachine _stateMachine;

        private Rigidbody2D _rigidbody2D;

        public bool IsGrounded { get; private set; }

        private void Awake()
        {
            _stateMachine = new StateMachine(isLogging: true);
            _rigidbody2D = playerView.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _stateMachine.Execute<PlayerStateBase>(new IdleState()).Forget();
        }

        private void Update()
        {
            IsGrounded = Physics2D.OverlapCircle(playerView.transform.position, groundCheckRadius, groundLayer) is not null;
        }

        public void Jump()
        {
            Debug.Log("Jump");
            // 점프 물리 적용
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


        public EventChain OnEventHandle(IEvent param)
        {
            switch (param)
            {
                case PlayerJumpEvent:
                    Debug.Log("플레이어 점프");
                    
                    if (!IsGrounded) 
                        return EventChain.Break;
                    
                    _stateMachine.Execute<PlayerStateBase>(new JumpState(this)).Forget();
                    return EventChain.Break;

                case PlayerHealthChangedEvent:
                    // UI 갱신
                    return EventChain.Break;

                case PlayerDiedEvent:
                    // 죽음 처리
                    return EventChain.Break;
            }

            return EventChain.Continue;
        }
    }
}