using Feature.Common;
using UnityEngine;

namespace Scenes.Feature.Game.Controller
{
    public struct BulletOnTriggerEvent : IEvent
    {
    }

    public class BulletHitDetector : MonoBehaviour, IMonoEventDispatcher
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.Emit<BulletOnTriggerEvent>();
            }
        }
    }
}
