using UnityEngine;

namespace Feature.Game.View
{
    /// <summary>
    /// 플레이어의 시각적 표현과 물리 동작을 담당
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator animator;
        
        [Header("Physics")]
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckPoint;
        
        [Header("Effects")]
        [SerializeField] private ParticleSystem jumpEffect;
        [SerializeField] private ParticleSystem deathEffect;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip deathSound;

        private void Awake()
        {
            if (rb2D == null) rb2D = GetComponent<Rigidbody2D>();
            if (animator == null) animator = GetComponent<Animator>();
            if (groundCheckPoint == null) groundCheckPoint = transform;
        }

        /// <summary>
        /// 지면 체크 (Controller에서 호출)
        /// </summary>
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer) != null;
        }

        /// <summary>
        /// 점프 물리 적용
        /// </summary>
        public void ApplyJumpForce(float jumpForce)
        {
            if (rb2D != null)
            {
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        /// <summary>
        /// 점프 애니메이션과 이펙트
        /// </summary>
        public void PlayJumpAnimation()
        {
            Debug.Log("플레이어 점프 애니메이션 재생");
            
            if (animator != null)
                animator.SetTrigger("Jump");
                
            if (jumpEffect != null)
                jumpEffect.Play();
                
            if (audioSource != null && jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }

        /// <summary>
        /// 아이들 애니메이션
        /// </summary>
        public void PlayIdleAnimation()
        {
            Debug.Log("플레이어 아이들 애니메이션 재생");
            
            if (animator != null)
                animator.SetTrigger("Idle");
        }

        /// <summary>
        /// 죽음 애니메이션과 이펙트
        /// </summary>
        public void PlayDeathAnimation()
        {
            Debug.Log("플레이어 죽음 애니메이션 재생");
            
            if (animator != null)
                animator.SetTrigger("Death");
                
            if (deathEffect != null)
                deathEffect.Play();
                
            if (audioSource != null && deathSound != null)
                audioSource.PlayOneShot(deathSound);
        }

        /// <summary>
        /// 체력 UI 업데이트 (간단한 예시)
        /// </summary>
        public void UpdateHealthUI(int current, int max)
        {
            Debug.Log($"Health UI 업데이트: {current}/{max}");
            // UI 업데이트 로직
        }

        /// <summary>
        /// 데미지 받은 표현
        /// </summary>
        public void PlayDamageEffect()
        {
            Debug.Log("데미지 받음 이펙트");
            // 빨간 플래시, 화면 흔들림 등
        }

        // 디버그용 Gizmo
        private void OnDrawGizmosSelected()
        {
            if (groundCheckPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
            }
        }
        
    }
}