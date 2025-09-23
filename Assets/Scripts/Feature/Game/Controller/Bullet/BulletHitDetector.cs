using Feature.Common;
using Feature.Game.Controller.Scenes.Feature.Game.Events;
using UnityEngine;

namespace Feature.Game.Controller.Bullet
{
    /// <summary>
    /// 총알의 충돌 감지를 담당하는 컴포넌트
    /// </summary>
    public class BulletHitDetector : MonoBehaviour, IMonoEventDispatcher
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Hit Player");
                
                // 로컬 이벤트 발행 (상위 BulletController에게)
                this.Emit(new BulletHitEvent(transform.position, "Player"));
            }
        }
    }
}
