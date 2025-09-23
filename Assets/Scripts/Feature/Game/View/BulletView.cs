using UnityEngine;

namespace Feature.Game.View
{
    /// <summary>
    /// 총알의 시각적 표현과 물리 동작을 담당
    /// </summary>
    public class BulletView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitSound;

        private Rigidbody2D rb2D;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 총알 움직임 처리 (Controller에서 호출)
        /// </summary>
        public void MoveBullet(float speed)
        {
            if (rb2D != null)
            {
                rb2D.linearVelocity = new Vector2(Time.deltaTime * -speed, 0);
            }
        }

        /// <summary>
        /// 충돌 이펙트 재생
        /// </summary>
        public void PlayHitEffect()
        {
            if (hitEffect != null)
                hitEffect.Play();

            if (audioSource != null && hitSound != null)
                audioSource.PlayOneShot(hitSound);
        }

        /// <summary>
        /// 총알 파괴 애니메이션
        /// </summary>
        public void PlayDestroyEffect()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.color = Color.clear;

            PlayHitEffect();
        }
    }
}